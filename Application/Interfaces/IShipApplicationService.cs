using Domain.ShipAggregate;

namespace Application.Interfaces;
public interface IShipApplicationService {
    public Task RegisterShipAsync(Ship ship);

    public Task<Ship?> GetRandomShipAsync(bool allowSkins);
}
