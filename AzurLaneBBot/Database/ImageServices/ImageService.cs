using AzurLaneBBot.Database.DatabaseServices;

namespace AzurLaneBBot.Database.ImageServices {
    public class ImageService : IImageService {

        private IDatabaseService _databaseService;

        public ImageService(IDatabaseService databaseService) {
            _databaseService = databaseService;
        }

        public ShipImage? GetImage(string shipName) {
            var ship = _databaseService.GetBBPShip(shipName);

            if (ship == null || string.IsNullOrEmpty(ship?.ImageUrl)) return null;

            return new ShipImage(shipName, ship.ImageUrl);
        }

        public bool StoreImage(string imagePath) {
            throw new NotImplementedException();
        }
    }
}
