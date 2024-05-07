namespace AzurLaneBBot.Database.ImageServices {
    public interface IImageService {

        public bool StoreImage(string shipName);

        public ShipImage? GetImage(string shipName);
    }

    public class ShipImage(string shipName, string imageURL, string filePath) {
        public string ShipName { get; private set; } = shipName;
        public string ImageUrl { get; private set; } = imageURL;
        public string FilePath { get; private set; } = filePath;
    }
}
