using Domain.ShipAggregate.Enums;

namespace Domain.ShipAggregate;

public class Ship : IEntity {
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required Rarity Rarity { get; set; }
}
