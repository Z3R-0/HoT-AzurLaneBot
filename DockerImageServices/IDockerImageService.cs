namespace DockerImageServices {
    public interface IDockerImageService {
        public ImageStoreResponse StoreImage(string image);

        public Image RetrieveImage(string imagePath);
    }

    public class ImageStoreResponse(bool isSuccess, string errorMessage) {
        public bool IsSuccess { get; private set; } = isSuccess;
        public string ErrorMessage { get; private set; } = errorMessage;
    }

    public class Image(string imagePath) {
        public string ImagePath { get; private set; } = imagePath;
    }
}
