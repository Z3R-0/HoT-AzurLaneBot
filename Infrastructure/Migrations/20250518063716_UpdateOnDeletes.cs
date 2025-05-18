using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOnDeletes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkinVisualTraits_Skins_SkinId",
                table: "SkinVisualTraits");

            migrationBuilder.DropForeignKey(
                name: "FK_SkinVisualTraits_VisualTraits_VisualTraitId",
                table: "SkinVisualTraits");

            migrationBuilder.AddForeignKey(
                name: "FK_SkinVisualTraits_Skins_SkinId",
                table: "SkinVisualTraits",
                column: "SkinId",
                principalTable: "Skins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SkinVisualTraits_VisualTraits_VisualTraitId",
                table: "SkinVisualTraits",
                column: "VisualTraitId",
                principalTable: "VisualTraits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkinVisualTraits_Skins_SkinId",
                table: "SkinVisualTraits");

            migrationBuilder.DropForeignKey(
                name: "FK_SkinVisualTraits_VisualTraits_VisualTraitId",
                table: "SkinVisualTraits");

            migrationBuilder.AddForeignKey(
                name: "FK_SkinVisualTraits_Skins_SkinId",
                table: "SkinVisualTraits",
                column: "SkinId",
                principalTable: "Skins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SkinVisualTraits_VisualTraits_VisualTraitId",
                table: "SkinVisualTraits",
                column: "VisualTraitId",
                principalTable: "VisualTraits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
