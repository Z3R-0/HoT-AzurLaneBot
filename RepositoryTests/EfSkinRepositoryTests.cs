using AutoFixture;
using Domain.SkinAggregate;
using FluentAssertions;
using Infrastructure.Repositories;

namespace RepositoryTests;

public class EfSkinRepositoryTests : RepositoryTestBase {
    private readonly EfSkinRepository _repository;
    private readonly Fixture _fixture;

    public EfSkinRepositoryTests() {
        _repository = new EfSkinRepository(DbContext);
        _fixture = new Fixture();
    }

    public class GetByNameAsync : EfSkinRepositoryTests {
        [Fact]
        public async Task ShouldReturnSkin_WhenSkinExists() {
            // Arrange
            var skin = _fixture.Build<Skin>()
                               .With(s => s.Name, "TestSkin")
                               .Create();
            DbContext.Skins.Add(skin);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetByNameAsync("TestSkin");

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("TestSkin");
        }

        [Fact]
        public async Task ShouldReturnNull_WhenSkinDoesNotExist() {
            // Act
            var result = await _repository.GetByNameAsync("NonExistentSkin");

            // Assert
            result.Should().BeNull();
        }
    }

    public class AddAsync : EfSkinRepositoryTests {
        [Fact]
        public async Task ShouldAddSkinToDatabase() {
            // Arrange
            var skin = _fixture.Build<Skin>()
                               .With(s => s.Name, "NewSkin")
                               .Create();

            // Act
            await _repository.AddAsync(skin);
            await DbContext.SaveChangesAsync();

            // Assert
            var result = await DbContext.Skins.FindAsync(skin.Id);
            result.Should().NotBeNull();
            result!.Name.Should().Be("NewSkin");
        }
    }

    public class DeleteAsync : EfSkinRepositoryTests {
        [Fact]
        public async Task ShouldRemoveSkinFromDatabase() {
            // Arrange
            var skin = _fixture.Build<Skin>()
                               .With(s => s.Name, "SkinToDelete")
                               .Create();
            DbContext.Skins.Add(skin);
            await DbContext.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(skin.Id);
            await DbContext.SaveChangesAsync();

            // Assert
            var result = await DbContext.Skins.FindAsync(skin.Id);
            result.Should().BeNull();
        }
    }
}