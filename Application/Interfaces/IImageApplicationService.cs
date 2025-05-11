using Application.DTO;

namespace Application.Interfaces;
public interface IImageApplicationService {

    public Task<(bool, string)> RegisterImage(UploadShipImage shipImage, bool overwriteExisting);

    public Task<ShipImage?> GetImage(string shipName);
}
