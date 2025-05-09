using Domain.ShipAggregate;

namespace Application;
public interface IShipApplicationService {
    public Task RegisterShipAsync(Ship ship);
}
