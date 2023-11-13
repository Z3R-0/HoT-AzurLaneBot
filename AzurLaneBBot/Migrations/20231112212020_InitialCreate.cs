using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzurLaneBBot.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BOOBA_BOT_PROJECT",
                columns: table => new
                {
                    Rarity = table.Column<string>(type: "TEXT", nullable: true),
                    IsSkinOf = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CupSize = table.Column<string>(type: "TEXT", nullable: true),
                    CoverageType = table.Column<string>(type: "TEXT", nullable: true),
                    Shape = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BOOBA_BOT_PROJECT");
        }
    }
}
