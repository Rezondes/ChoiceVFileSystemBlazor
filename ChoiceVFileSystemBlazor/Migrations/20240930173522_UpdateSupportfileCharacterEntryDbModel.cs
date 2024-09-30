using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoiceVFileSystemBlazor.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSupportfileCharacterEntryDbModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountName",
                table: "SupportfileCharacterEntryDbModels",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DiscordId",
                table: "SupportfileCharacterEntryDbModels",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountName",
                table: "SupportfileCharacterEntryDbModels");

            migrationBuilder.DropColumn(
                name: "DiscordId",
                table: "SupportfileCharacterEntryDbModels");
        }
    }
}
