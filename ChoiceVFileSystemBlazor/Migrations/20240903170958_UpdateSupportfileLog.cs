using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoiceVFileSystemBlazor.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSupportfileLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorAccountId",
                table: "SupportfileLogsDbModels");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "SupportfileLogsDbModels",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "LastModifierAccountId",
                table: "SupportfileLogsDbModels",
                newName: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "SupportfileLogsDbModels",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "SupportfileLogsDbModels",
                newName: "LastModifierAccountId");

            migrationBuilder.AddColumn<int>(
                name: "CreatorAccountId",
                table: "SupportfileLogsDbModels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
