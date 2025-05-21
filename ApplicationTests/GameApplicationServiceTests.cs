using Application;
using Application.DTO;
using Application.Interfaces;
using AutoFixture;
using Domain.ShipAggregate;
using Domain.SkinAggregate;
using FluentAssertions;
using Moq;

namespace ApplicationTests;

public class GameApplicationServiceTests {
    private readonly Mock<ISkinApplicationService> _skinServiceMock = new();
    private readonly Mock<IShipApplicationService> _shipServiceMock = new();
    private readonly Fixture _fixture = new();
    private readonly GameApplicationService _service;

    public GameApplicationServiceTests() {
        _service = new GameApplicationService(_skinServiceMock.Object, _shipServiceMock.Object);
    }

    public class StartGuessShipGameAsync : GameApplicationServiceTests {
        [Fact]
        public async Task ShouldReturnGameResult_WhenAllDependenciesSucceed() {
            // Arrange
            var skin = _fixture.Build<Skin>()
                               .With(s => s.Name, "TestSkin")
                               .With(s => s.ShipId, Guid.NewGuid())
                               .Create();
            var image = _fixture.Build<ShipImage>()
                                .With(i => i.ShipName, skin.Name)
                                .Create();
            var ship = _fixture.Build<Ship>()
                               .With(s => s.Id, skin.ShipId)
                               .Create();

            _skinServiceMock.Setup(s => s.GetRandomSkin(It.IsAny<bool>())).ReturnsAsync(skin);
            _skinServiceMock.Setup(s => s.GetImageAsync(skin.Name)).ReturnsAsync(image);
            _shipServiceMock.Setup(s => s.GetByIdAsync(skin.ShipId)).ReturnsAsync(ship);

            // Act
            var result = await _service.StartGuessShipGameAsync(true);

            // Assert
            result.Should().NotBeNull();
            result!.Ship.Should().Be(ship);
            result.ImageUrl.Should().Be(image.ImageUrl);
            result.ImageFilePath.Should().Be(image.FilePath);
            result.Prompt.Should().Be("What is the name of this ship?");
        }

        [Fact]
        public async Task ShouldReturnNull_WhenNoSkinIsFound() {
            // Arrange
            _skinServiceMock.Setup(s => s.GetRandomSkin(It.IsAny<bool>())).ReturnsAsync((Skin?)null);

            // Act
            var result = await _service.StartGuessShipGameAsync(true);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task ShouldReturnNull_WhenNoImageIsFound() {
            // Arrange
            var skin = _fixture.Build<Skin>()
                               .With(s => s.Name, "TestSkin")
                               .With(s => s.ShipId, Guid.NewGuid())
                               .Create();

            _skinServiceMock.Setup(s => s.GetRandomSkin(It.IsAny<bool>())).ReturnsAsync(skin);
            _skinServiceMock.Setup(s => s.GetImageAsync(skin.Name)).ReturnsAsync((ShipImage?)null);

            // Act
            var result = await _service.StartGuessShipGameAsync(true);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task ShouldReturnNull_WhenNoShipIsFound() {
            // Arrange
            var skin = _fixture.Build<Skin>()
                               .With(s => s.Name, "TestSkin")
                               .With(s => s.ShipId, Guid.NewGuid())
                               .Create();
            var image = _fixture.Build<ShipImage>()
                                .With(i => i.ShipName, skin.Name)
                                .Create();

            _skinServiceMock.Setup(s => s.GetRandomSkin(It.IsAny<bool>())).ReturnsAsync(skin);
            _skinServiceMock.Setup(s => s.GetImageAsync(skin.Name)).ReturnsAsync(image);
            _shipServiceMock.Setup(s => s.GetByIdAsync(skin.ShipId)).ReturnsAsync((Ship?)null);

            // Act
            var result = await _service.StartGuessShipGameAsync(true);

            // Assert
            result.Should().BeNull();
        }
    }
}