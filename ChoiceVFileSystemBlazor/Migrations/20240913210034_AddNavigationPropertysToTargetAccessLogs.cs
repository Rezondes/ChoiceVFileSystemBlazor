using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoiceVFileSystemBlazor.Migrations
{
    /// <inheritdoc />
    public partial class AddNavigationPropertysToTargetAccessLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TargetAccessId",
                table: "AccessLogsDbModels",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AccessLogsDbModels_TargetAccessId",
                table: "AccessLogsDbModels",
                column: "TargetAccessId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessLogsDbModels_AccessDbModels_TargetAccessId",
                table: "AccessLogsDbModels",
                column: "TargetAccessId",
                principalTable: "AccessDbModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessLogsDbModels_AccessDbModels_TargetAccessId",
                table: "AccessLogsDbModels");

            migrationBuilder.DropIndex(
                name: "IX_AccessLogsDbModels_TargetAccessId",
                table: "AccessLogsDbModels");

            migrationBuilder.AlterColumn<string>(
                name: "TargetAccessId",
                table: "AccessLogsDbModels",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
