using Domain.ShipAggregate;
using Domain.SkinAggregate;
using Domain.VisualTraitAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
public interface IApplicationDbContext {
    DbSet<Ship> Ships { get; }
    DbSet<Skin> Skins { get; }
    DbSet<VisualTrait> VisualTraits { get; }
    DbSet<SkinVisualTrait> SkinVisualTraits { get; }

    DbSet<T> Set<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
