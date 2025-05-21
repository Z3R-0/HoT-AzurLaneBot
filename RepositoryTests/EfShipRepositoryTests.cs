using AutoFixture;
using Domain.ShipAggregate;
using FluentAssertions;
using Infrastructure.Repositories;

namespace RepositoryTests;

public class EfShipRepositoryTests : RepositoryTestBase {
    private readonly EfShipRepository _repository;
    private readonly Fixture _fixture;

    public EfShipRepositoryTests() {
        _repository = new EfShipRepository(DbContext);
        _fixture = new Fixture();
    }

    public class GetByNameAsync : EfShipRepositoryTests {
        [Fact]
        public async Task ShouldReturnShip_WhenShipExists() {
            // Arrange
            var ship = _fixture.Build<Ship>()
                               .With(s => s.Name, "TestShip")
                               .Create();
            DbContext.Ships.Add(ship);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetByNameAsync("TestShip");

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("TestShip");
        }

        [Fact]
        public async Task ShouldReturnNull_WhenShipDoesNotExist() {
            // Act
            var result = await _repository.GetByNameAsync("NonExistentShip");

            // Assert
            result.Should().BeNull();
        }
    }

    public class AddAsync : EfShipRepositoryTests {
        [Fact]
        public async Task ShouldAddShipToDatabase() {
            // Arrange
            var ship = _fixture.Build<Ship>()
                               .With(s => s.Name, "NewShip")
                               .Create();

            // Act
            await _repository.AddAsync(ship);
            await DbContext.SaveChangesAsync();

            // Assert
            var result = await DbContext.Ships.FindAsync(ship.Id);
            result.Should().NotBeNull();
            result!.Name.Should().Be("NewShip");
        }
    }

    public class DeleteAsync : EfShipRepositoryTests {
        [Fact]
        public async Task ShouldRemoveShipFromDatabase() {
            // Arrange
            var ship = _fixture.Build<Ship>()
                               .With(s => s.Name, "ShipToDelete")
                               .Create();
            DbContext.Ships.Add(ship);
            await DbContext.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(ship.Id);
            await DbContext.SaveChangesAsync();

            // Assert
            var result = await DbContext.Ships.FindAsync(ship.Id);
            result.Should().BeNull();
        }
    }
}