using AzurLaneBBot.Database.DatabaseServices;
using System.Reflection;

namespace AzurLaneBBot.Database.ImageServices {
    public class ImageService(IDatabaseService databaseService) : IImageService {

        protected static readonly string _imageLocation = $"{Path.DirectorySeparatorChar}" +
                                                "Images" +
                                                $"{Path.DirectorySeparatorChar}";
        private IDatabaseService _databaseService = databaseService;

        public ShipImage? GetImage(string shipName) {
            var ship = _databaseService.GetBBPShip(shipName);

            if (ship == null || string.IsNullOrEmpty(ship?.ImageUrl)) return null;

            var filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + ship.ImageUrl;

            return new ShipImage(shipName, $"attachment://{shipName}.png", filePath);
        }

        public bool RegisterImage(string shipName) {
            var ship = _databaseService.GetBBPShip(shipName);

            if (ship == null) return false;

            return _databaseService.UpdateBBShipImageURL(shipName, ShipNameToImageLocation(shipName));
        }

        private static string ShipNameToImageLocation(string shipName) {
            return _imageLocation + shipName + ".png";
        }
    }
}
