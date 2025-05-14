using Application.DTO;
using Application.Interfaces;

namespace Application;
public class GameApplicationService(IShipApplicationService shipApplicationService, ISkinApplicationService imageService) : IGameApplicationService {
    private readonly IShipApplicationService _shipApplicationService = shipApplicationService;
    private readonly ISkinApplicationService _imageService = imageService;

    public async Task<GuessShipGameResult?> StartGuessShipGameAsync(bool allowSkins) {
        var ship = await _shipApplicationService.GetRandomShipAsync(allowSkins);
        if (ship == null) return null;

        var image = await _imageService.GetImageAsync(ship.Name);
        if (image == null) return null;

        return new GuessShipGameResult {
            Ship = ship,
            ImageUrl = image.ImageUrl,
            ImageFilePath = image.FilePath,
            Prompt = allowSkins ? "What is the name of this ship or skin?" : "What is the name of this ship?",
        };
    }
}
