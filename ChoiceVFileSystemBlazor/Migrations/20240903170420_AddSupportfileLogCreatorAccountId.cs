using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoiceVFileSystemBlazor.Migrations
{
    /// <inheritdoc />
    public partial class AddSupportfileLogCreatorAccountId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifierAccountId",
                table: "SupportfileLogsDbModels",
                newName: "LastModifierAccountId");

            migrationBuilder.AddColumn<int>(
                name: "CreatorAccountId",
                table: "SupportfileLogsDbModels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorAccountId",
                table: "SupportfileLogsDbModels");

            migrationBuilder.RenameColumn(
                name: "LastModifierAccountId",
                table: "SupportfileLogsDbModels",
                newName: "ModifierAccountId");
        }
    }
}
