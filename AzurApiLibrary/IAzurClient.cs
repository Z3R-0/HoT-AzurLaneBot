using Jan0660.AzurAPINet.Ships;

namespace AzurApiLibrary {
    public interface IAzurClient {

        public Task<Ship> GetShipAsync(string ShipName);
    }
}
