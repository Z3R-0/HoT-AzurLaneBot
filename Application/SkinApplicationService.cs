using Application.DTO;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.SkinAggregate;
using System.Reflection;

namespace Application;
public class SkinApplicationService(IUnitOfWork unitOfWork,
    ISkinRepository skinRepository,
    IShipRepository shipRepository)
    : ISkinApplicationService {

    protected static readonly string _imageLocation = $"{Path.DirectorySeparatorChar}" +
                                            "Images" +
                                            $"{Path.DirectorySeparatorChar}";
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISkinRepository _skinRepository = skinRepository;
    private readonly IShipRepository _shipRepository = shipRepository;

    public async Task<ShipImage?> GetImageAsync(string skinName) {
        var skin = (await _skinRepository.GetByNameAsync(skinName));

        if (skin == null) return null;

        var filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + _imageLocation + skin.ImageUrl + ".png";

        return new ShipImage {
            ShipName = skinName,
            ImageUrl = $"attachment://{skinName}.png",
            FilePath = filePath
        };
    }

    public async Task<(bool, string)> RegisterSkinAsync(RegisterSkin dto) {
        var skinName = dto.SkinName ?? dto.ShipName;

        var ship = await _shipRepository.GetByNameAsync(dto.ShipName);
        if (ship == null)
            return (false, $"No ship found with name '{dto.ShipName}'.");

        var skin = new Skin {
            Name = skinName,
            Ship = ship,
            ShipId = ship.Id,
            ImageUrl = skinName,
            CoverageType = dto.CoverageType,
            CupSize = dto.CupSize,
            Shape = dto.Shape
        };

        try {
            await SaveImage(dto.ImageData, skinName);
            await _skinRepository.AddAsync(skin);
            await _unitOfWork.SaveChangesAsync();
        } catch (Exception ex) {
            return (false, $"An error occurred while saving the skin to the database, error: {ex.Message}");
        }
        return (true, $"Skin '{skinName}' successfully registered for ship '{dto.ShipName}'.");
    }

    private static async Task SaveImage(byte[] imageData, string fileName) {
        var imageDirectory = Path.Combine(AppContext.BaseDirectory, "Images");
        Directory.CreateDirectory(imageDirectory);

        var imagePath = Path.Combine(imageDirectory, fileName);

        await File.WriteAllBytesAsync(imagePath, imageData);
    }
}
