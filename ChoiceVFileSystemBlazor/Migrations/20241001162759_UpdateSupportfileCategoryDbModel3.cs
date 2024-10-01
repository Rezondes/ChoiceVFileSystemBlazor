using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoiceVFileSystemBlazor.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSupportfileCategoryDbModel3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportfileDbModels_SupportfileCategoryDbModels_CategoryId",
                table: "SupportfileDbModels");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportfileDbModels_SupportfileCategoryDbModels_CategoryId",
                table: "SupportfileDbModels",
                column: "CategoryId",
                principalTable: "SupportfileCategoryDbModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportfileDbModels_SupportfileCategoryDbModels_CategoryId",
                table: "SupportfileDbModels");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportfileDbModels_SupportfileCategoryDbModels_CategoryId",
                table: "SupportfileDbModels",
                column: "CategoryId",
                principalTable: "SupportfileCategoryDbModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
