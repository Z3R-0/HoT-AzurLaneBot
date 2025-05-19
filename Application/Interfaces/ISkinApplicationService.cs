using Application.DTO;
using Domain.SkinAggregate;

namespace Application.Interfaces;
public interface ISkinApplicationService {
    public Task<(bool isSuccess, string? Errors)> RegisterSkinAsync(RegisterSkin dto);
    public Task<(bool isSuccess, string? Errors)> DeleteSkinAsync(string skinName);
    public Task<Skin?> GetByNameAsync(string skinName);
    public Task<ShipImage?> GetImageAsync(string shipName);
}
