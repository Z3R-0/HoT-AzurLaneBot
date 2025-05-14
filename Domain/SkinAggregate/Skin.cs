using Domain.ShipAggregate;
using Domain.SkinAggregate.Enums;

namespace Domain.SkinAggregate;
public class Skin : IEntity {
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string ImageUrl { get; set; }
    public required CoverageType CoverageType { get; set; }
    public required CupSize CupSize { get; set; }
    public required Shape Shape { get; set; }
    public Guid ShipId { get; set; }
    public required Ship Ship { get; set; }
}
