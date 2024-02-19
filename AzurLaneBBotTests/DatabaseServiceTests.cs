using AutoFixture;
using AzurLaneBBot.Database.DatabaseServices;
using AzurLaneBBot.Database.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AzurLaneBBotTests {
    [TestClass]
    public class DatabaseServiceTests {

        [TestMethod]
        public void RetrieveExistingDbEntry_HappyFlow_HasData_ReturnsShip() {
            // Arrange
            var expectedShipName = "TestShip";
            var actualShipName = "TestShip";

            var mockDatabaseContext = GenerateMockContext(new List<BoobaBotProject>() {
                new BoobaBotProject() {
                    Rarity = "Test",
                    IsSkinOf = "",
                    Name = actualShipName,
                    CupSize = "T",
                    CoverageType = "TestType",
                    Shape = "T-shaped",
                    ImageUrl = "testURL"
                }
            }.AsQueryable());
            var dbService = new AzurDbContextDatabaseService(mockDatabaseContext.Object);

            // Act
            var entry = dbService.GetBBPShip(expectedShipName);

            // Assert
            entry.Should().NotBeNull();
            entry?.Name.Should().BeEquivalentTo(actualShipName);
        }

        [TestMethod]
        public void RetrieveNonexistingDbEntry_HappyFlow_HasData_ReturnsNull() {
            // Arrange
            var expectedShipName = "TestSSShip";
            var actualShipName = "TestShip";

            var mockDatabaseContext = GenerateMockContext(new List<BoobaBotProject>() {
                new BoobaBotProject() {
                    Rarity = "Test",
                    IsSkinOf = "",
                    Name = actualShipName,
                    CupSize = "T",
                    CoverageType = "TestType",
                    Shape = "T-shaped",
                    ImageUrl = "testURL"
                }
            }.AsQueryable());
            var dbService = new AzurDbContextDatabaseService(mockDatabaseContext.Object);

            // Act
            var entry = dbService.GetBBPShip(expectedShipName);

            // Assert
            entry.Should().BeNull();
            entry?.Name.Should().NotBeEquivalentTo(actualShipName);
        }

        [TestMethod]
        public void RetrieveDbEntry_HappyFlow_NoData_ReturnsNull() {
            // Arrange
            var expectedShipName = "TestShip";
            var actualShipName = "";

            var mockDatabaseContext = GenerateMockContext(new List<BoobaBotProject>() {
                new BoobaBotProject() {
                    Rarity = "Test",
                    IsSkinOf = "",
                    Name = actualShipName,
                    CupSize = "T",
                    CoverageType = "TestType",
                    Shape = "T-shaped",
                    ImageUrl = "testURL"
                }
            }.AsQueryable());
            var dbService = new AzurDbContextDatabaseService(mockDatabaseContext.Object);

            // Act
            var entry = dbService.GetBBPShip(expectedShipName);

            // Assert
            entry.Should().BeNull();
            entry?.Name.Should().NotBeEquivalentTo(actualShipName);
        }

        [TestMethod]
        public void UpdateImageUrl_HappyFlow_WithData_ReturnsTrue() {
            // Arrange
            var newUrl = "www.thisisaNewURL.com";
            var oldUrl = "www.thisisanOldURL.com";
            var shipName = "TestShip";

            var mockDatabaseContext = GenerateMockContext(new List<BoobaBotProject>() {
                new BoobaBotProject() {
                    Rarity = "Test",
                    IsSkinOf = "",
                    Name = shipName,
                    CupSize = "T",
                    CoverageType = "TestType",
                    Shape = "T-shaped",
                    ImageUrl = oldUrl
                }
            }.AsQueryable());
            var dbService = new AzurDbContextDatabaseService(mockDatabaseContext.Object);

            // Act
            var result = dbService.UpdateBBShipImageURL(shipName, newUrl);

            // Assert
            result.Should().BeTrue();
            dbService.GetBBPShip(shipName)?.ImageUrl.Should().BeEquivalentTo(newUrl);
        }

        [TestMethod]
        public void UpdateImageUrl_HappyFlow_NoData_ReturnsFalse() {
            // Arrange
            var newUrl = "www.thisisaNewURL.com";
            var shipName = "TestShip";

            var mockDatabaseContext = GenerateMockContext(new List<BoobaBotProject>().AsQueryable());
            var dbService = new AzurDbContextDatabaseService(mockDatabaseContext.Object);

            // Act
            var result = dbService.UpdateBBShipImageURL(newUrl, shipName);

            // Assert
            result.Should().BeFalse();
            dbService.GetBBPShip(shipName).Should().BeNull();
        }

        [TestMethod]
        public void UpdateBBPShip_HappyFlow_ReturnsTrue() {
            // Arrange
            var fixture = new Fixture();

            var originalEntry = fixture.Create<BoobaBotProject>();
            var updatedEntry = fixture.Create<BoobaBotProject>();

            updatedEntry.Name = originalEntry.Name;
            updatedEntry.Id = originalEntry.Id;

            var mockEntityEntryWrapper = new Mock<IEntityEntryWrapper<BoobaBotProject>>();
            mockEntityEntryWrapper.Setup(x => x.Update(It.IsAny<BoobaBotProject>(), It.IsAny<BoobaBotProject>()))
                                 .Callback((BoobaBotProject original, BoobaBotProject updated) => {
                                     // Update properties of the original entry with the updated entry
                                     original.Name = updated.Name;
                                     original.Rarity = updated.Rarity;
                                     original.CupSize = updated.CupSize;
                                     originalEntry.CoverageType = updated.CoverageType;
                                     original.Shape = updated.Shape;
                                     original.IsSkinOf = updated.IsSkinOf;
                                     original.ImageUrl = updated.ImageUrl;
                                 }).Returns((BoobaBotProject original, BoobaBotProject updated) => original);

            var mockDbContext = GenerateMockContext(new List<BoobaBotProject>() { originalEntry }.AsQueryable());
            var dbService = new AzurDbContextDatabaseService(mockDbContext.Object, mockEntityEntryWrapper.Object);

            // Act
            var result = dbService.UpdateBBShip(updatedEntry);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(updatedEntry);
        }

        [TestMethod]
        public void AddBBPShip_HappyFlow_ReturnsTrue() {
            // Arrange
            var fixture = new Fixture();
            var newEntry = fixture.Create<BoobaBotProject>();

            var mockEntityEntryWrapper = new Mock<IEntityEntryWrapper<BoobaBotProject>>();
            mockEntityEntryWrapper.Setup(x => x.Add(It.IsAny<BoobaBotProject>())).Returns((BoobaBotProject entity) => entity);

            var mockDbContext = new Mock<AzurlanedbContext>();
            var dbService = new AzurDbContextDatabaseService(mockDbContext.Object, mockEntityEntryWrapper.Object);

            // Act
            var result = dbService.AddBBShip(newEntry);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(newEntry);
        }

        private static Mock<AzurlanedbContext> GenerateMockContext(IQueryable<BoobaBotProject> data) {
            var mockDatabaseContext = new Mock<AzurlanedbContext>();
            var mockDbSet = new Mock<DbSet<BoobaBotProject>>();

            // Setup mocked database service
            mockDbSet.As<IQueryable<BoobaBotProject>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<BoobaBotProject>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<BoobaBotProject>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<BoobaBotProject>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator);

            // Setup mocked database context to return the mocked DbSet
            mockDatabaseContext.Setup(x => x.BoobaBotProjects).Returns(mockDbSet.Object);

            return mockDatabaseContext;
        }
    }
}