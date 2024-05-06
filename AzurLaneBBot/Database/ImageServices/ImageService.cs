using AzurLaneBBot.Database.DatabaseServices;

namespace AzurLaneBBot.Database.ImageServices {
    public class ImageService(IDatabaseService databaseService) : IImageService {

        protected const string _imageLocation = "\\Images\\";
        private IDatabaseService _databaseService = databaseService;

        public ShipImage? GetImage(string shipName) {
            var ship = _databaseService.GetBBPShip(shipName);

            if (ship == null || string.IsNullOrEmpty(ship?.ImageUrl)) return null;

            return new ShipImage(shipName, ship.ImageUrl);
        }

        public bool StoreImage(string shipName) {
            var ship = _databaseService.GetBBPShip(shipName);

            if (ship == null) return false;

            return _databaseService.UpdateBBShipImageURL(shipName, ShipNameToImageLocation(shipName));
        }

        private static string ShipNameToImageLocation(string shipName) {
            return _imageLocation + shipName + ".png";
        }
    }
}
