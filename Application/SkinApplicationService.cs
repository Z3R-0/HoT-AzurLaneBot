using Application.DTO;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.SkinAggregate;

namespace Application;
public class SkinApplicationService(
    IUnitOfWork unitOfWork,
    ISkinRepository skinRepository,
    IShipRepository shipRepository,
    IImageStorageService imageStorageService) : ISkinApplicationService {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISkinRepository _skinRepository = skinRepository;
    private readonly IShipRepository _shipRepository = shipRepository;
    private readonly IImageStorageService _imageStorageService = imageStorageService;

    public async Task<ShipImage?> GetImageAsync(string skinName) {
        var skin = await _skinRepository.GetByNameAsync(skinName);

        if (skin == null) return null;

        var filePath = _imageStorageService.GetImagePath(skin.ImageUrl.Replace("/Images/", ""));

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
            return (false, $"[Input Error]: No ship found with name '{dto.ShipName}'.");

        var existingSkin = await _skinRepository.GetByNameAsync(skinName);

        var fileName = skinName + ".png";
        var imageUrl = $"/Images/{fileName}";

        Skin skin;
        if (existingSkin != null) {
            skin = existingSkin;
        } else {
            skin = new Skin {
                Name = skinName,
                Ship = ship,
                ShipId = ship.Id,
                ImageUrl = imageUrl,
                CoverageType = dto.CoverageType,
                CupSize = dto.CupSize,
                Shape = dto.Shape
            };
        }

        // Always update these fields, even for an existing skin (overwrite logic)
        skin.ImageUrl = imageUrl;
        skin.CoverageType = dto.CoverageType;
        skin.CupSize = dto.CupSize;
        skin.Shape = dto.Shape;

        await _imageStorageService.SaveImageAsync(dto.ImageData, fileName);

        if (existingSkin == null)
            await _skinRepository.AddAsync(skin);
        else
            await _skinRepository.UpdateAsync(skin);

        await _unitOfWork.SaveChangesAsync();

        return (true, $"Skin '{skinName}' successfully registered for ship '{dto.ShipName}'.");
    }
}