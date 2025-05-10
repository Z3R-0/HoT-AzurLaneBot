using Domain.ShipAggregate;

namespace Domain.Interfaces;
public interface IShipRepository {
    Task<Ship?> GetByIdAsync(Guid id);
    Task<Ship?> GetByNameAsync(string name);
    Task<IEnumerable<Ship>> GetAllAsync(bool includeSkins = true);
    Task AddAsync(Ship ship);
    Task UpdateAsync(Ship ship);
    Task DeleteAsync(Guid id);
}
