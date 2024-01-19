namespace AzurLaneBBot.Database.ImageServices {
    public interface IImageService {

        public bool StoreImage(string imagePath);

        public string GetImagePath(string relatedShip);
    }
}
