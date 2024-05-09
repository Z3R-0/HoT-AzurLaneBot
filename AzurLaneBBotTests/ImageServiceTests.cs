using AzurLaneBBot.Database.DatabaseServices;
using AzurLaneBBot.Database.ImageServices;
using AzurLaneBBot.Database.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AzurLaneBBotTests {
    [TestClass]
    public class ImageServiceTests {
        [TestMethod]
        public void RetrieveImageURL_HappyFlow_ReturnsURL() {
            // Arrange 
            var shipName = "TestShip";
            var expectedURL = "theExpectedURL";
            var actualURL = "theExpectedURL";

            Mock<AzurlanedbContext> mockDatabaseContext = GenerateMockContext(new List<BoobaBotProject>() {
                new BoobaBotProject() {
                    Rarity = "Test",
                    IsSkinOf = "",
                    Name = shipName,
                    CupSize = "T",
                    CoverageType = "TestType",
                    Shape = "T-shaped",
                    ImageUrl = actualURL
                }
            }.AsQueryable());

            var dbService = new AzurDbContextDatabaseService(mockDatabaseContext.Object);

            // Act
            var imageService = new ImageService(dbService);
            var result = imageService.GetImage(shipName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(string.Equals(expectedURL, actualURL));
        }

        [TestMethod]
        public void RetrieveImageURL_UnhappyFlow_ReturnsNull() {
            // Arrange 
            var shipName = "TestShip";
            var expectedURL = "theExpectedURL";

            Mock<AzurlanedbContext> mockDatabaseContext = GenerateMockContext(new List<BoobaBotProject>() { }.AsQueryable());

            var dbService = new AzurDbContextDatabaseService(mockDatabaseContext.Object);

            // Act
            var imageService = new ImageService(dbService);
            var result = imageService.GetImage(shipName);

            // Assert
            Assert.IsNull(result);
            Assert.IsFalse(string.Equals(expectedURL, result?.ImageUrl));
        }

        [TestMethod]
        public void StoreImage_HappyFlow_ReturnsSuccess() {
            // Arrange
            var shipName = "TestShip";
            var imagePath = $"\\Images\\{shipName}.png";

            var mockDatabaseContext = GenerateMockContext(new List<BoobaBotProject>() {
                new BoobaBotProject() {
                    Rarity = "Test",
                    IsSkinOf = "",
                    Name = shipName,
                    CupSize = "T",
                    CoverageType = "TestType",
                    Shape = "T-shaped",
                    ImageUrl = null
                }
            }.AsQueryable());

            var dbService = new AzurDbContextDatabaseService(mockDatabaseContext.Object);

            // Act
            var imageService = new ImageService(dbService);
            var result = imageService.RegisterImage(shipName);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(imagePath, dbService.GetBBPShip(shipName)?.ImageUrl);
        }

        [TestMethod]
        public void StoreImage_UnhappyFlow_ReturnsFalse() {
            // Arrange
            var shipName = "TestShip";
            var imagePath = $"\\Images\\{shipName}.png";

            var mockDatabaseContext = GenerateMockContext(new List<BoobaBotProject>() { }.AsQueryable());

            var dbService = new AzurDbContextDatabaseService(mockDatabaseContext.Object);

            // Act
            var imageService = new ImageService(dbService);
            var result = imageService.RegisterImage(shipName);

            // Assert
            Assert.IsFalse(result);
            Assert.AreNotEqual(imagePath, dbService.GetBBPShip(shipName)?.ImageUrl);
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
