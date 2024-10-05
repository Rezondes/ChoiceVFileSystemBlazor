using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoiceVFileSystemBlazor.Migrations
{
    /// <inheritdoc />
    public partial class RewriteSupportfileToFilesWithType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nummer",
                table: "SupportfileCategoryDbModels",
                newName: "Number");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "SupportfileDbModels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "SupportfileDbModels");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "SupportfileCategoryDbModels",
                newName: "Nummer");
        }
    }
}
