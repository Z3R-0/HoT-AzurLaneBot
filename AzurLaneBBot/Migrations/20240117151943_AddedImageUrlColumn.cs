using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzurLaneBBot.Migrations
{
    /// <inheritdoc />
    public partial class AddedImageUrlColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "BOOBA_BOT_PROJECT",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "BOOBA_BOT_PROJECT");
        }
    }
}
