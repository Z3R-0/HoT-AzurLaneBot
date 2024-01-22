using Jan0660.AzurAPINet;
using Jan0660.AzurAPINet.Ships;
using ReshDiscordNetLibrary;

namespace AzurApiLibrary {
    public class AzurClient : IAzurClient {

        private AzurAPIClient azurApiClient = new AzurAPIClient(new AzurAPIClientOptions());

        public async Task<Ship> GetShipAsync(string ShipName) {
            var isUpdateAvailable = await azurApiClient.DatabaseUpdateAvailableAsync();

            if (isUpdateAvailable) {
                Logger.Log("There is an update available for the 3rd party API");
            }

            // reload/update cached data
            await azurApiClient.ReloadCachedAsync();

            // reload cached data to update it
            await azurApiClient.ReloadEverythingAsync();

            var testShip = azurApiClient.getShipByEnglishName(ShipName.ToLower().Trim());
            return testShip;
        }
    }
}