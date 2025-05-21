using Domain.SkinAggregate.Enums;

namespace Application.DTO;
public class RegisterSkin {
    public required string ShipName { get; set; }
    public string? SkinName { get; set; } // optional, fallback to base skin
    public required byte[] ImageData { get; set; }
    public required CoverageType CoverageType { get; set; }
    public required CupSize CupSize { get; set; }
    public required Shape Shape { get; set; }
}
