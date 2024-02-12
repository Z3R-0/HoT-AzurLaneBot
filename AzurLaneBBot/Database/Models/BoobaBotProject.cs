using System.ComponentModel.DataAnnotations;

namespace AzurLaneBBot.Database.Models;

public partial class BoobaBotProject {
    [Key]
    public int Id { get; set; }

    public string? Rarity { get; set; }

    public string? IsSkinOf { get; set; }

    public string? Name { get; set; }

    public string? CupSize { get; set; }

    public string? CoverageType { get; set; }

    public string? Shape { get; set; }

    public string? ImageUrl { get; set; }
}
