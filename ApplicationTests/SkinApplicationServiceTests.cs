using Application;
using Application.DTO;
using Application.Interfaces;
using AutoFixture;
using Domain.Interfaces;
using Domain.ShipAggregate;
using Domain.SkinAggregate;
using FluentAssertions;
using Moq;

namespace ApplicationTests;

public class SkinApplicationServiceTests {
    private readonly Fixture _fixture = new();
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly Mock<ISkinRepository> _skinRepoMock = new();
    private readonly Mock<IShipRepository> _shipRepoMock = new();
    private readonly Mock<IImageStorageService> _imageStorageMock = new();
    private readonly SkinApplicationService _service;

    public SkinApplicationServiceTests() {
        _service = new SkinApplicationService(
            _uowMock.Object,
            _skinRepoMock.Object,
            _shipRepoMock.Object,
            _imageStorageMock.Object);
    }
    public class RegisterSkinAsync : SkinApplicationServiceTests {
        [Fact]
        public async Task ShouldReturnFalse_When_ShipDoesNotExist() {
            // Arrange
            var dto = _fixture.Build<RegisterSkin>()
                              .With(x => x.ShipName, "UnknownShip")
                              .Create();

            _shipRepoMock.Setup(r => r.GetByNameAsync(dto.ShipName))
                         .ReturnsAsync((Ship?)null);

            // Act
            var (success, message) = await _service.RegisterSkinAsync(dto);

            // Assert
            success.Should().BeFalse();
            message.Should().Contain("No ship found");

            _skinRepoMock.Verify(r => r.AddAsync(It.IsAny<Skin>()), Times.Never);
            _uowMock.Verify(u => u.SaveChangesAsync(default), Times.Never);
            _imageStorageMock.Verify(i => i.SaveImageAsync(It.IsAny<byte[]>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ShouldAddSkin_When_ValidNewSkin() {
            // Arrange
            var ship = _fixture.Build<Ship>()
                               .With(s => s.Name, "ExistingShip")
                               .Create();

            var dto = _fixture.Build<RegisterSkin>()
                              .With(x => x.ShipName, ship.Name)
                              .With(x => x.SkinName, "NewSkin")
                              .Create();

            _shipRepoMock.Setup(r => r.GetByNameAsync(ship.Name)).ReturnsAsync(ship);
            _skinRepoMock.Setup(r => r.GetByNameAsync(dto.SkinName!)).ReturnsAsync((Skin?)null);
            _imageStorageMock.Setup(i => i.SaveImageAsync(dto.ImageData, dto.SkinName!)).Returns(Task.CompletedTask);

            // Act
            var (success, message) = await _service.RegisterSkinAsync(dto);

            // Assert
            success.Should().BeTrue();
            message.Should().BeNull();

            _skinRepoMock.Verify(r => r.AddAsync(It.IsAny<Skin>()), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
            _imageStorageMock.Verify(i => i.SaveImageAsync(dto.ImageData, dto.SkinName! + ".png"), Times.Once);
        }

        [Fact]
        public async Task ShouldUpdateSkin_When_SkinAlreadyExists() {
            // Arrange
            var ship = _fixture.Build<Ship>()
                               .With(s => s.Name, "Ship1").Create();
            var existingSkin = _fixture.Build<Skin>()
                                       .With(s => s.Name, "ExistingSkin")
                                       .With(s => s.ShipId, ship.Id)
                                       .Create();

            var dto = _fixture.Build<RegisterSkin>()
                              .With(d => d.SkinName, existingSkin.Name)
                              .With(d => d.ShipName, ship.Name)
                              .Create();

            _shipRepoMock.Setup(r => r.GetByNameAsync(ship.Name)).ReturnsAsync(ship);
            _skinRepoMock.Setup(r => r.GetByNameAsync(existingSkin.Name)).ReturnsAsync(existingSkin);

            // Act
            var (success, message) = await _service.RegisterSkinAsync(dto);

            // Assert
            success.Should().BeTrue();
            message.Should().BeNull();
            _skinRepoMock.Verify(r => r.UpdateAsync(existingSkin), Times.Once);
            _skinRepoMock.Verify(r => r.AddAsync(It.IsAny<Skin>()), Times.Never);
            _uowMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task ShouldReturnFalse_When_ImageSavingFails() {
            // Arrange
            var ship = _fixture.Build<Ship>()
                               .With(s => s.Name, "ShipX").Create();
            var dto = _fixture.Build<RegisterSkin>()
                              .With(d => d.ShipName, ship.Name)
                              .With(d => d.SkinName, "SkinX")
                              .Create();

            var newSkin = new Skin {
                Name = dto.SkinName!,
                ImageUrl = $"/Images/{dto.SkinName!}.png",
                CoverageType = dto.CoverageType,
                CupSize = dto.CupSize,
                Shape = dto.Shape,
                ShipId = ship.Id
            };

            _shipRepoMock.Setup(r => r.GetByNameAsync(ship.Name)).ReturnsAsync(ship);
            _skinRepoMock.Setup(r => r.GetByNameAsync(dto.SkinName!)).ReturnsAsync((Skin?)null);
            _skinRepoMock.Setup(r => r.AddAsync(It.IsAny<Skin>())).Callback<Skin>(s => newSkin = s);
            _imageStorageMock
                .Setup(x => x.SaveImageAsync(It.IsAny<byte[]>(), It.IsAny<string>()))
                .ThrowsAsync(new IOException("Disk full"));

            // Act
            var action = async () => await _service.RegisterSkinAsync(dto);

            // Assert
            await action.Should().ThrowAsync<IOException>();
            _skinRepoMock.Verify(r => r.AddAsync(It.Is<Skin>(s => s.Name == dto.SkinName)), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(default), Times.Never);
            _imageStorageMock.Verify(i => i.SaveImageAsync(dto.ImageData, dto.SkinName! + ".png"), Times.Once);
        }

        [Fact]
        public async Task ShouldUseShipNameAsFallback_When_SkinNameIsNull() {
            // Arrange
            var ship = _fixture.Build<Ship>()
                               .With(s => s.Name, "FallbackShip").Create();
            var dto = _fixture.Build<RegisterSkin>()
                              .Without(d => d.SkinName)
                              .With(d => d.ShipName, ship.Name)
                              .Create();

            _shipRepoMock.Setup(r => r.GetByNameAsync(ship.Name)).ReturnsAsync(ship);
            _skinRepoMock.Setup(r => r.GetByNameAsync(ship.Name)).ReturnsAsync((Skin?)null);

            // Act
            var (success, message) = await _service.RegisterSkinAsync(dto);

            // Assert
            success.Should().BeTrue();
            _skinRepoMock.Verify(r => r.AddAsync(It.Is<Skin>(s => s.Name == ship.Name)), Times.Once);
        }
    }

    public class GetImageAsync : SkinApplicationServiceTests {
        [Fact]
        public async Task ShouldReturnNull_When_SkinNotFound_InGetImage() {
            // Arrange
            _skinRepoMock.Setup(r => r.GetByNameAsync("MissingSkin")).ReturnsAsync((Skin?)null);

            // Act
            var result = await _service.GetImageAsync("MissingSkin");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task ShouldReturnShipImage_When_SkinExists() {
            // Arrange
            var skin = _fixture.Build<Skin>()
                               .With(s => s.Name, "Skin1")
                               .With(s => s.ImageUrl, "/Images/Skin1.png")
                               .Create();

            _skinRepoMock.Setup(r => r.GetByNameAsync("Skin1")).ReturnsAsync(skin);
            _imageStorageMock.Setup(i => i.GetImagePath(skin.ImageUrl.Replace("/Images/", ""))).Returns(skin.Name + ".png");

            // Act
            var result = await _service.GetImageAsync("Skin1");

            // Assert
            result.Should().NotBeNull();
            result!.ShipName.Should().Be("Skin1");
            result.ImageUrl.Should().Be("attachment://Skin1.png");
            result.FilePath.Should().Contain("Skin1.png");
        }
    }

    public class GetRandomShipAsync : SkinApplicationServiceTests {
        [Fact]
        public async Task ShouldReturnNull_When_NoSkinsExist() {
            // Arrange
            _skinRepoMock.Setup(r => r.GetAllAsync())
                         .ReturnsAsync([]);

            // Act
            var result = await _service.GetRandomSkin(allowSkins: true);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task ShouldReturnRandomSkin_When_SkinExist() {
            // Arrange
            var ship = _fixture.Build<Ship>().Create();
            var skin = _fixture.Build<Skin>().Create();

            skin.ShipId = ship.Id;

            _shipRepoMock.Setup(r => r.GetAllAsync())
                         .ReturnsAsync([ship]);
            _skinRepoMock.Setup(r => r.GetAllAsync())
                         .ReturnsAsync([skin]);


            // Act
            var result = await _service.GetRandomSkin(allowSkins: true);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(skin);
        }
    }
}