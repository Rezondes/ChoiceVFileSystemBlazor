using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoiceVFileSystemBlazor.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBugTrackerTaskItemDbModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByDiscordId",
                table: "BugTrackerTaskItemDbModels",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByDiscordName",
                table: "BugTrackerTaskItemDbModels",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByDiscordId",
                table: "BugTrackerTaskItemDbModels");

            migrationBuilder.DropColumn(
                name: "CreatedByDiscordName",
                table: "BugTrackerTaskItemDbModels");
        }
    }
}
