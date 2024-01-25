namespace AzurLaneBBot.Database.ImageServices {
    public interface IImageService {

        public bool StoreImage(string shipName);

        public ShipImage? GetImage(string shipName);
    }

    public class ShipImage {
        public string ShipName { get; private set; }
        public string ImagePath { get; private set; }

        public ShipImage(string shipName, string imageURL) {
            ShipName = shipName;
            ImagePath = imageURL;
        }
    }
}
