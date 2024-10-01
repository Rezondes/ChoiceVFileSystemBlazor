using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoiceVFileSystemBlazor.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSupportfileCategoryDbModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportfileCategoryDbModel_SupportfileDbModels_SupportfileId",
                table: "SupportfileCategoryDbModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupportfileCategoryDbModel",
                table: "SupportfileCategoryDbModel");

            migrationBuilder.DropIndex(
                name: "IX_SupportfileCategoryDbModel_SupportfileId",
                table: "SupportfileCategoryDbModel");

            migrationBuilder.RenameTable(
                name: "SupportfileCategoryDbModel",
                newName: "SupportfileCategoryDbModels");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "SupportfileDbModels",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SupportfileId",
                table: "SupportfileCategoryDbModels",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupportfileCategoryDbModels",
                table: "SupportfileCategoryDbModels",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SupportfileDbModels_CategoryId",
                table: "SupportfileDbModels",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportfileDbModels_SupportfileCategoryDbModels_CategoryId",
                table: "SupportfileDbModels",
                column: "CategoryId",
                principalTable: "SupportfileCategoryDbModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportfileDbModels_SupportfileCategoryDbModels_CategoryId",
                table: "SupportfileDbModels");

            migrationBuilder.DropIndex(
                name: "IX_SupportfileDbModels_CategoryId",
                table: "SupportfileDbModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupportfileCategoryDbModels",
                table: "SupportfileCategoryDbModels");

            migrationBuilder.RenameTable(
                name: "SupportfileCategoryDbModels",
                newName: "SupportfileCategoryDbModel");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "SupportfileDbModels",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SupportfileId",
                table: "SupportfileCategoryDbModel",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupportfileCategoryDbModel",
                table: "SupportfileCategoryDbModel",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SupportfileCategoryDbModel_SupportfileId",
                table: "SupportfileCategoryDbModel",
                column: "SupportfileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportfileCategoryDbModel_SupportfileDbModels_SupportfileId",
                table: "SupportfileCategoryDbModel",
                column: "SupportfileId",
                principalTable: "SupportfileDbModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
