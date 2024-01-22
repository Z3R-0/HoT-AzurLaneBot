using AzurApiLibrary;
using AzurLaneBBot.Database.DatabaseServices;
using AzurLaneBBot.Database.Models;
using AzurLaneBBot.Modules.Commands;
using Jan0660.AzurAPINet.Ships;
using Moq;

namespace AzurLaneBBotTests {
    [TestClass]
    public class TestCommandTests : TestCommands {
        public TestCommandTests(AzurClient azurClient, AzurlanedbContext azurlanedbContext) : base(azurClient, azurlanedbContext) {
        }

        [TestMethod]
        public async Task HandleTestSlash_HappyFlow_ReturnsShip() {
            // Arrange
            var mockApiClient = new Mock<AzurClient>();
            var mockDatabaseService = new Mock<AzurDbContextDatabaseService>();
            var mockEntry = new Mock<BoobaBotProject>();
            var mockApiShip = new Mock<Ship>();

            // Setup mocked database entry
            mockEntry.Setup(x => x.Name).Returns("TestShip").Verifiable();
            // Setup mocked database service calls
            mockDatabaseService.Setup(x => x.GetBBPShip(It.IsAny<string>())).Returns(mockEntry.Object).Verifiable();
            // Setup mocked API calls
            mockApiClient.Setup(x => x.GetShipAsync(It.IsAny<string>())).ReturnsAsync(mockApiShip.Object).Verifiable();

            this._dbService = mockDatabaseService.Object;

            // Act
            var testCommand = new TestCommands(mockApiClient.Object, new Mock<AzurlanedbContext>().Object);

            // Assert
            await testCommand.HandleTestSlash("TestShip");
        }
    }
}