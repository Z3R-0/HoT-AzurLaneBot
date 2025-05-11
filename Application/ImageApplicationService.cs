using Application.DTO;
using Application.Interfaces;
using Domain.Interfaces;
using System.Reflection;

namespace Application;
public class ImageApplicationService(IShipRepository shipRepository, ISkinRepository skinRepository) : IImageApplicationService {

    protected static readonly string _imageLocation = $"{Path.DirectorySeparatorChar}" +
                                            "Images" +
                                            $"{Path.DirectorySeparatorChar}";
    private IShipRepository _shipRepository = shipRepository;
    private ISkinRepository _skinRepository = skinRepository;

    public async Task<ShipImage?> GetImage(string shipName) {
        var skin = (await _shipRepository.GetByNameAsync(shipName))
            ?.Skins.FirstOrDefault(skin => skin.Name.Equals(shipName, StringComparison.OrdinalIgnoreCase));

        if (skin == null) return null;

        var filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + _imageLocation + skin.ImageUrl + ".png";

        return new ShipImage {
            ShipName = shipName,
            ImageUrl = $"attachment://{shipName}.png",
            FilePath = filePath
        };
    }

    public async Task<(bool, string)> RegisterImage(UploadShipImage shipImage, bool overwriteExisting) {
        if (!overwriteExisting && (await _skinRepository.GetByNameAsync(shipImage.Name) != null))
            return (false, "Image already exists for this skin, to replace it you must overwrite it");

        var baseShip = _shipRepository.GetByNameAsync(shipImage.ShipName);

        if (baseShip == null)
            return (false, $"No ship found with name: '{shipImage.ShipName}'");

        // TODO: Take logic currently found in HandleUploadImageSlash in ManagementCommand.cs and re-implement here with new setup

        return (true, string.Empty);
    }
}
