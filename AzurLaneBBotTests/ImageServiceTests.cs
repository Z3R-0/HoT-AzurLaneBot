using AzurLaneBBot.Database.ImageServices;

namespace AzurLaneBBotTests {
    [TestClass]
    public class ImageServiceTests {
        [TestMethod]
        public void RetrieveImageURL_HappyFlow_ReturnsURL() {
            // Arrange 
            var shipName = "thisIsAValidShipName";

            // Act
            var imageService = new ImageService();
            var result = imageService.GetImage(shipName);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void StoreImage_HappyFlow_ReturnsSuccess() {
            // Arrange 
            var imagePath = "thisIsAValidURL";

            // Act
            var imageService = new ImageService();
            var result = imageService.StoreImage(imagePath);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
