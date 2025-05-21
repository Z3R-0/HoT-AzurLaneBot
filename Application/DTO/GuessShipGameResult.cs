using Domain.ShipAggregate;

namespace Application.DTO;
public class GuessShipGameResult {
    public required Ship Ship { get; set; }
    public required string ImageUrl { get; set; }
    public required string ImageFilePath { get; set; }
    public required string Prompt { get; set; }
}
