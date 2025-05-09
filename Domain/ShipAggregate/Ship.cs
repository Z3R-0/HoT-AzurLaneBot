using Domain.ShipAggregate.Enums;
using Domain.SkinAggregate;

namespace Domain.ShipAggregate;

public class Ship {
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required Rarity Rarity { get; set; }
    public ICollection<Skin> Skins { get; set; } = [];
}
