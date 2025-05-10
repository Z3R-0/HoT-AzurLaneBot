using Application.DTO;

namespace Application;
public class GameApplicationService(IShipApplicationService shipApplicationService) : IGameApplicationService {
    private readonly IShipApplicationService _shipApplicationService = shipApplicationService;

    public async Task<GuessShipGameResult?> StartGuessShipGameAsync(bool allowSkins) {
        var ship = await _shipApplicationService.GetRandomShip(allowSkins);
        if (ship == null) return null;

        var image = _imageService.GetImage(ship.Name); // TODO
        return new GuessShipGameResult {
            Ship = ship,
            ImageUrl = image.ImageUrl,
            ImageFilePath = image.FilePath,
            Prompt = allowSkins ? "What is the name of this ship or skin?" : "What is the name of this ship?",
        };
    }
}
