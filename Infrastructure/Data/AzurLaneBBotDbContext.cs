using Domain.ShipAggregate;
using Domain.SkinAggregate;
using Domain.VisualTraitAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
public class AzurLaneBBotDbContext(DbContextOptions<AzurLaneBBotDbContext> options) : DbContext(options), IApplicationDbContext {
    public DbSet<Ship> Ships => Set<Ship>();
    public DbSet<Skin> Skins => Set<Skin>();
    public DbSet<VisualTrait> VisualTraits => Set<VisualTrait>();
    public DbSet<SkinVisualTrait> SkinVisualTraits => Set<SkinVisualTrait>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Skin>(entity => {
            entity.HasKey(e => e.Id);
            entity
                .Property(e => e.CoverageType)
                .HasConversion<string>();

            entity
                .Property(e => e.CupSize)
                .HasConversion<string>();

            entity
                .Property(e => e.Shape)
                .HasConversion<string>();

            entity
                .HasOne<Ship>()
                .WithMany()
                .HasForeignKey(e => e.ShipId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Ship>(entity => {
            entity.HasKey(e => e.Id);
            entity
                .Property(e => e.Rarity)
                .HasConversion<string>();
        });

        modelBuilder.Entity<VisualTrait>(entity => {
            entity.HasKey(e => e.Id);
            entity
                .Property(e => e.TraitType)
                .HasConversion<string>();
        });

        modelBuilder.Entity<SkinVisualTrait>(entity => {
            entity.HasKey(e => e.Id);
            entity
                .HasOne(e => e.Skin)
                .WithMany()
                .HasForeignKey(e => e.SkinId)
                .OnDelete(DeleteBehavior.Cascade);

            entity
                .HasOne(e => e.VisualTrait)
                .WithMany()
                .HasForeignKey(e => e.VisualTraitId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
