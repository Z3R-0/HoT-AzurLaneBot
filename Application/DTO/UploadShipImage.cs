using Domain.SkinAggregate.Enums;

namespace Application.DTO;
public class UploadShipImage {
    public required string Name { get; set; }
    public required string ShipName { get; set; }
    public required CoverageType CoverageType { get; set; }
    public required CupSize CupSize { get; set; }
    public required Shape Shape { get; set; }
}
