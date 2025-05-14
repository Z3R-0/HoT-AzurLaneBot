namespace Application.Interfaces;
public interface IImageStorageService {
    Task SaveImageAsync(byte[] imageData, string fileName);
    string GetImagePath(string fileName);
}
