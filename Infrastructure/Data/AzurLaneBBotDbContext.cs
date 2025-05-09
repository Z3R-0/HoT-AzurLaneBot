using Domain.ShipAggregate;
using Domain.SkinAggregate;
using Domain.VisualTraitAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
public class AzurLaneBBotDbContext(DbContextOptions<AzurLaneBBotDbContext> options) : DbContext(options) {
    public DbSet<Ship> Ships => Set<Ship>();
    public DbSet<Skin> Skins => Set<Skin>();
    public DbSet<VisualTrait> VisualTraits => Set<VisualTrait>();
    public DbSet<SkinVisualTrait> SkinVisualTraits => Set<SkinVisualTrait>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
    }
}
