using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoiceVFileSystemBlazor.Migrations
{
    /// <inheritdoc />
    public partial class AddNavigationPropertysToSupportfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatedByAccessId",
                table: "SupportfileDbModels",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SupportfileDbModels_CreatedByAccessId",
                table: "SupportfileDbModels",
                column: "CreatedByAccessId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportfileDbModels_AccessDbModels_CreatedByAccessId",
                table: "SupportfileDbModels",
                column: "CreatedByAccessId",
                principalTable: "AccessDbModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportfileDbModels_AccessDbModels_CreatedByAccessId",
                table: "SupportfileDbModels");

            migrationBuilder.DropIndex(
                name: "IX_SupportfileDbModels_CreatedByAccessId",
                table: "SupportfileDbModels");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByAccessId",
                table: "SupportfileDbModels",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
