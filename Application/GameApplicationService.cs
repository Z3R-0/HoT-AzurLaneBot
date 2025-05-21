using Application.DTO;
using Application.Interfaces;

namespace Application;
public class GameApplicationService(ISkinApplicationService skinApplicationService, IShipApplicationService shipApplicationService) : IGameApplicationService {
    private readonly ISkinApplicationService _skinApplicationService = skinApplicationService;
    private readonly IShipApplicationService _shipApplicationService = shipApplicationService;

    public async Task<GuessShipGameResult?> StartGuessShipGameAsync(bool allowSkins) {
        var skin = await _skinApplicationService.GetRandomSkin(allowSkins);
        if (skin == null) return null;

        var image = await _skinApplicationService.GetImageAsync(skin.Name);
        if (image == null) return null;

        var ship = await _shipApplicationService.GetByIdAsync(skin.ShipId);
        if (ship == null) return null;

        return new GuessShipGameResult {
            Ship = ship,
            ImageUrl = image.ImageUrl,
            ImageFilePath = image.FilePath,
            Prompt = "What is the name of this ship?",
        };
    }
}
