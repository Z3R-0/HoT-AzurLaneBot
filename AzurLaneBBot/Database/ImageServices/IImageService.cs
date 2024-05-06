namespace AzurLaneBBot.Database.ImageServices {
    public interface IImageService {

        public bool StoreImage(string shipName);

        public ShipImage? GetImage(string shipName);
    }

    public class ShipImage(string shipName, string imageURL) {
        public string ShipName { get; private set; } = shipName;
        public string ImagePath { get; private set; } = imageURL;
    }
}
