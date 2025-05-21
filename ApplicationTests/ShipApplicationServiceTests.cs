using Application;
using Application.DTO;
using AutoFixture;
using Domain.Interfaces;
using Domain.ShipAggregate;
using Domain.ShipAggregate.Enums;
using FluentAssertions;
using Moq;

namespace ApplicationTests;

public class ShipApplicationServiceTests {
    private readonly Fixture _fixture = new();
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly Mock<IShipRepository> _shipRepoMock = new();
    private readonly ShipApplicationService _service;

    public ShipApplicationServiceTests() {
        _service = new ShipApplicationService(
            _shipRepoMock.Object,
            _uowMock.Object);
    }

    public class RegisterShipAsync : ShipApplicationServiceTests {
        [Fact]
        public async Task ShouldReturnFalse_When_ShipAlreadyExists() {
            // Arrange
            var dto = _fixture.Build<RegisterShip>()
                              .With(x => x.ShipName, "ExistingShip")
                              .Create();

            var existingShip = _fixture.Build<Ship>()
                                       .With(s => s.Name, dto.ShipName)
                                       .Create();

            _shipRepoMock.Setup(r => r.GetByNameAsync(dto.ShipName))
                         .ReturnsAsync(existingShip);

            // Act
            var (success, message) = await _service.RegisterShipAsync(dto);

            // Assert
            success.Should().BeFalse();
            message.Should().Contain("A ship with the name");

            _shipRepoMock.Verify(r => r.AddAsync(It.IsAny<Ship>()), Times.Never);
            _uowMock.Verify(u => u.SaveChangesAsync(default), Times.Never);
        }

        [Fact]
        public async Task ShouldAddShip_When_ShipDoesNotExist() {
            // Arrange
            var dto = _fixture.Build<RegisterShip>()
                              .With(x => x.ShipName, "NewShip")
                              .With(x => x.Rarity, Rarity.SR)
                              .Create();

            _shipRepoMock.Setup(r => r.GetByNameAsync(dto.ShipName))
                         .ReturnsAsync((Ship?)null);

            // Act
            var (success, message) = await _service.RegisterShipAsync(dto);

            // Assert
            success.Should().BeTrue();
            message.Should().BeNull();

            _shipRepoMock.Verify(r => r.AddAsync(It.Is<Ship>(s => s.Name == dto.ShipName && s.Rarity == dto.Rarity)), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }
    }

    public class GetRandomShipAsync : ShipApplicationServiceTests {
        [Fact]
        public async Task ShouldReturnNull_When_NoShipsExist() {
            // Arrange
            _shipRepoMock.Setup(r => r.GetAllAsync(It.IsAny<bool>()))
                         .ReturnsAsync(Enumerable.Empty<Ship>());

            // Act
            var result = await _service.GetRandomSkin(allowSkins: true);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task ShouldReturnRandomShip_When_ShipsExist() {
            // Arrange
            var ships = _fixture.CreateMany<Ship>(5).ToList();

            _shipRepoMock.Setup(r => r.GetAllAsync(It.IsAny<bool>()))
                         .ReturnsAsync(ships);

            // Act
            var result = await _service.GetRandomSkin(allowSkins: true);

            // Assert
            result.Should().NotBeNull();
            ships.Should().Contain(result);
        }
    }
}