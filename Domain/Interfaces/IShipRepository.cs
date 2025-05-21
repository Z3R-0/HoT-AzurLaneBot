using Domain.ShipAggregate;

namespace Domain.Interfaces;
public interface IShipRepository : IGenericRepository<Ship> {
    Task<Ship?> GetByNameAsync(string name);
}
