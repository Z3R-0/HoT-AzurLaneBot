using Domain.SkinAggregate;

namespace Domain.Interfaces;
public interface ISkinRepository : IGenericRepository<Skin> {
    Task<Skin?> GetByNameAsync(string name);
}
