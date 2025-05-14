using Application.DTO;

namespace Application.Interfaces;
public interface ISkinApplicationService {

    public Task<(bool, string)> RegisterSkinAsync(RegisterSkin dto);

    public Task<ShipImage?> GetImageAsync(string shipName);
}
