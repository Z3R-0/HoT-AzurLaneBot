using AzurLaneBBot.Database.Models;

namespace AzurLaneBBot.Database.DatabaseServices {
    public interface IDatabaseService {
        public IEnumerable<BoobaBotProject?> GetAllBBPShips();

        public BoobaBotProject? GetBBPShip(string name);

        public bool UpdateBBShipImageURL(string shipToUpdateName, string imageUrl);
    }
}
