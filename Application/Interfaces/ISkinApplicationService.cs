using Application.DTO;

namespace Application.Interfaces;
public interface ISkinApplicationService {

    public Task<(bool isSuccess, string? Errors)> RegisterSkinAsync(RegisterSkin dto);

    public Task<ShipImage?> GetImageAsync(string shipName);
}
