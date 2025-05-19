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

    public async Task<(bool isSuccess, string? Errors)> DeleteSkinAsync(string skinName) {
        var skin = await _skinRepository.GetByNameAsync(skinName);
        if (skin == null) {
            return (false, $"[Input Error]: No skin found with name '{skinName}'.");
        }

        await _skinRepository.DeleteAsync(skin.Id);
        await _unitOfWork.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool isSuccess, string? Errors)> RegisterSkinAsync(RegisterSkin dto) {
        var skinName = dto.SkinName ?? dto.ShipName;

        var ship = await _shipRepository.GetByNameAsync(dto.ShipName);
        if (ship == null)
            return (false, $"[Input Error]: No ship found with name '{dto.ShipName}'.");
        var existingSkin = await _skinRepository.GetByNameAsync(skinName);

        var fileName = skinName + ".png";
        var imageUrl = $"/Images/{fileName}";

        if (existingSkin != null) {
            existingSkin.ImageUrl = imageUrl;
            existingSkin.CoverageType = dto.CoverageType;
            existingSkin.CupSize = dto.CupSize;
            existingSkin.Shape = dto.Shape;

            await _skinRepository.UpdateAsync(existingSkin);
        } else {
            var newSkin = new Skin {
                Name = skinName,
                ImageUrl = imageUrl,
                CoverageType = dto.CoverageType,
                CupSize = dto.CupSize,
                Shape = dto.Shape,
                ShipId = ship.Id,
            };

            await _skinRepository.AddAsync(newSkin);
        }

        await _imageStorageService.SaveImageAsync(dto.ImageData, fileName);

        await _unitOfWork.SaveChangesAsync();

        return (true, null);
    }

    public async Task<Skin?> GetByNameAsync(string skinName) {
        return await _skinRepository.GetByNameAsync(skinName);
    }

    public async Task<Skin?> GetByIdAsync(Guid skinId) {
        return await _skinRepository.GetByIdAsync(skinId);
    }
}