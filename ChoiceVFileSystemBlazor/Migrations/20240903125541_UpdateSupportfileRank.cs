using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoiceVFileSystemBlazor.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSupportfileRank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeededDeletePower",
                table: "Supportfiles");

            migrationBuilder.DropColumn(
                name: "NeededEditPower",
                table: "Supportfiles");

            migrationBuilder.DropColumn(
                name: "NeededSeeLogPower",
                table: "Supportfiles");

            migrationBuilder.RenameColumn(
                name: "NeededViewPower",
                table: "Supportfiles",
                newName: "MinRank");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MinRank",
                table: "Supportfiles",
                newName: "NeededViewPower");

            migrationBuilder.AddColumn<int>(
                name: "NeededDeletePower",
                table: "Supportfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NeededEditPower",
                table: "Supportfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NeededSeeLogPower",
                table: "Supportfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
