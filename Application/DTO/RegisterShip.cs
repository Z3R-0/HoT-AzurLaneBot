using Domain.ShipAggregate.Enums;

namespace Application.DTO;
public class RegisterShip {
    public required string ShipName { get; set; }
    public required Rarity Rarity { get; set; }
}
