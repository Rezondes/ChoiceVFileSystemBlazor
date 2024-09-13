using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoiceVFileSystemBlazor.Migrations
{
    /// <inheritdoc />
    public partial class AddNavigationPropertysToSupportfileLogsAndEntrys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccessId",
                table: "SupportfileLogsDbModels",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByAccessId",
                table: "SupportfileEntryDbModels",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportfileEntryDbModels_AccessDbModels_CreatedByAccessId",
                table: "SupportfileEntryDbModels",
                column: "CreatedByAccessId",
                principalTable: "AccessDbModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportfileLogsDbModels_AccessDbModels_AccessId",
                table: "SupportfileLogsDbModels",
                column: "AccessId",
                principalTable: "AccessDbModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportfileEntryDbModels_AccessDbModels_CreatedByAccessId",
                table: "SupportfileEntryDbModels");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportfileLogsDbModels_AccessDbModels_AccessId",
                table: "SupportfileLogsDbModels");

            migrationBuilder.DropIndex(
                name: "IX_SupportfileLogsDbModels_AccessId",
                table: "SupportfileLogsDbModels");

            migrationBuilder.DropIndex(
                name: "IX_SupportfileEntryDbModels_CreatedByAccessId",
                table: "SupportfileEntryDbModels");

            migrationBuilder.AlterColumn<string>(
                name: "AccessId",
                table: "SupportfileLogsDbModels",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByAccessId",
                table: "SupportfileEntryDbModels",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
