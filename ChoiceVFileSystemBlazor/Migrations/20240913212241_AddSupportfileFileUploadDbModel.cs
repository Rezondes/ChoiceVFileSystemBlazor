using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoiceVFileSystemBlazor.Migrations
{
    /// <inheritdoc />
    public partial class AddSupportfileFileUploadDbModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupportfileFileUploadDbModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntryId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContentType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Data = table.Column<byte[]>(type: "longblob", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportfileFileUploadDbModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportfileFileUploadDbModels_SupportfileEntryDbModels_Entry~",
                        column: x => x.EntryId,
                        principalTable: "SupportfileEntryDbModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SupportfileFileUploadDbModels_EntryId",
                table: "SupportfileFileUploadDbModels",
                column: "EntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupportfileFileUploadDbModels");
        }
    }
}
