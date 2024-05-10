using AzurLaneBBot.Database.Models;

namespace AzurLaneBBot.Database.DatabaseServices {
    public interface IDatabaseService {
        public IEnumerable<BoobaBotProject?> GetAllBBPShips();

        public BoobaBotProject? GetBBPShip(string name);

        public bool UpdateBBShipImageURL(string shipToUpdateName, string imageUrl);

        public BoobaBotProject UpdateBBShip(BoobaBotProject updatedShip);

        public BoobaBotProject UpdateBBShip(BoobaBotProject updatedShip, string originalName);

        public bool DeleteBBShip(string shipToDeleteName);

        public BoobaBotProject AddBBShip(BoobaBotProject shipToCreate);
    }
}
