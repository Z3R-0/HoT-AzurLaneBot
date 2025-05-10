using Domain.ShipAggregate;

namespace Application;
public interface IShipApplicationService {
    public Task RegisterShipAsync(Ship ship);

    public Task<Ship?> GetRandomShip(bool allowSkins);
}
