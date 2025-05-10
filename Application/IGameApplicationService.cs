using Application.DTO;

namespace Application;
public interface IGameApplicationService {
    public Task<GuessShipGameResult?> StartGuessShipGameAsync(bool allowSkins);
}
