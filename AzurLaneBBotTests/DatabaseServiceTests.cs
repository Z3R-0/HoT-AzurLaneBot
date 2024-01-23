using AzurLaneBBot.Database.DatabaseServices;
using AzurLaneBBot.Database.Models;
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

            Mock<AzurlanedbContext> mockDatabaseContext = GenerateMockContext(new List<BoobaBotProject>() {
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

            // Act
            var dbService = new AzurDbContextDatabaseService(mockDatabaseContext.Object);
            var entry = dbService.GetBBPShip(expectedShipName);

            // Assert
            Assert.IsNotNull(entry);
            Assert.AreEqual(entry.Name, actualShipName, ignoreCase: false);
        }

        [TestMethod]
        public void RetrieveNonexistingDbEntry_HappyFlow_HasData_ReturnsNull() {
            // Arrange
            var expectedShipName = "TestSSShip";
            var actualShipName = "TestShip";

            Mock<AzurlanedbContext> mockDatabaseContext = GenerateMockContext(new List<BoobaBotProject>() {
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

            // Act
            var dbService = new AzurDbContextDatabaseService(mockDatabaseContext.Object);
            var entry = dbService.GetBBPShip(expectedShipName);

            // Assert
            Assert.IsNull(entry);
            Assert.AreNotEqual(entry?.Name, actualShipName, ignoreCase: false);
        }

        [TestMethod]
        public void RetrieveDbEntry_HappyFlow_NoData_ReturnsNull() {
            // Arrange
            var expectedShipName = "TestShip";
            var actualShipName = "";

            Mock<AzurlanedbContext> mockDatabaseContext = GenerateMockContext(new List<BoobaBotProject>() {
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

            // Act
            var dbService = new AzurDbContextDatabaseService(mockDatabaseContext.Object);
            var entry = dbService.GetBBPShip(expectedShipName);

            // Assert
            Assert.IsNull(entry);
            Assert.AreNotEqual(entry?.Name, actualShipName, ignoreCase: false);
        }

        [TestMethod]
        public void UpdateImageUrl_HappyFlow_WithData_ReturnsTrue() {
            // Arrange
            var newUrl = "www.thisisaNewURL.com";
            var oldUrl = "www.thisisanOldURL.com";
            var shipName = "TestShip";

            Mock<AzurlanedbContext> mockDatabaseContext = GenerateMockContext(new List<BoobaBotProject>() {
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

            // Act
            var dbService = new AzurDbContextDatabaseService(mockDatabaseContext.Object);
            var result = dbService.UpdateBBShipImageURL(shipName, newUrl);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(newUrl, dbService.GetBBPShip(shipName)?.ImageUrl);
        }

        [TestMethod]
        public void UpdateImageUrl_HappyFlow_NoData_ReturnsFalse() {
            // Arrange
            var newUrl = "www.thisisaNewURL.com";
            var shipName = "TestShip";

            Mock<AzurlanedbContext> mockDatabaseContext = GenerateMockContext(new List<BoobaBotProject>() { }.AsQueryable());

            // Act
            var dbService = new AzurDbContextDatabaseService(mockDatabaseContext.Object);
            var result = dbService.UpdateBBShipImageURL(newUrl, shipName);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(dbService.GetBBPShip(shipName));
        }

        private static Mock<AzurlanedbContext> GenerateMockContext(IQueryable<BoobaBotProject> data) {
            var mockDatabaseContext = new Mock<AzurlanedbContext>();
            var mockDbSet = new Mock<DbSet<BoobaBotProject>>();

            // Setup mocked database service
            mockDbSet.As<IQueryable<BoobaBotProject>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<BoobaBotProject>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<BoobaBotProject>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<BoobaBotProject>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator);
            mockDatabaseContext.Setup(x => x.BoobaBotProjects).Returns(mockDbSet.Object).Verifiable();
            return mockDatabaseContext;
        }
    }
}