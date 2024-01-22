using AzurApiLibrary;

namespace AzurLaneBBotTests {
    [TestClass]
    public class AzurApiLibraryTests {
        [TestMethod]
        public async Task RetrieveExistingShip_HappyFlow_ReturnsShip() {
            // Arrange
            var testShipName = "Hakuryuu";
            var azurApiClient = new AzurClient();

            // Act
            var result = await azurApiClient.GetShipAsync(testShipName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Names.en, testShipName);
        }

        [TestMethod]
        public async Task RetrieveNonexistingShip_UnHappyFlow_ThrowsException() {
            // Arrange
            var testShipName = "NotAName";
            var azurApiClient = new AzurClient();

            // Act
            var result = await azurApiClient.GetShipAsync(testShipName);

            // Assert
            Assert.IsNull(result);
            Assert.AreNotEqual(result?.Names?.en, testShipName);
        }
    }
}
