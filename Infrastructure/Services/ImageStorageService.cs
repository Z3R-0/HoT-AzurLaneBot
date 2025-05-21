using Application.Interfaces;

namespace Infrastructure.Services;
public class ImageStorageService : IImageStorageService {
    private readonly string _imageDirectory;

    public ImageStorageService() {
        _imageDirectory = Path.Combine(AppContext.BaseDirectory, "Images");
        Directory.CreateDirectory(_imageDirectory);
    }

    public async Task SaveImageAsync(byte[] imageData, string fileName) {
        var imagePath = Path.Combine(_imageDirectory, fileName);
        await File.WriteAllBytesAsync(imagePath, imageData);
    }

    public string GetImagePath(string fileName) {
        return Path.Combine(_imageDirectory, fileName);
    }
}
