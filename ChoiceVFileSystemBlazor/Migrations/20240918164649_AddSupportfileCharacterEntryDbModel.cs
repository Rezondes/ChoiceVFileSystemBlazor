using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoiceVFileSystemBlazor.Migrations
{
    /// <inheritdoc />
    public partial class AddSupportfileCharacterEntryDbModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SupportfileId",
                table: "SupportfileLogsDbModels",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SupportfileId",
                table: "SupportfileEntryDbModels",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SupportfileCharacterEntryDbModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SupportfileId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    CharacterFirstName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CharacterLastName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportfileCharacterEntryDbModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportfileCharacterEntryDbModels_SupportfileDbModels_Suppor~",
                        column: x => x.SupportfileId,
                        principalTable: "SupportfileDbModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SupportfileLogsDbModels_SupportfileId",
                table: "SupportfileLogsDbModels",
                column: "SupportfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportfileEntryDbModels_SupportfileId",
                table: "SupportfileEntryDbModels",
                column: "SupportfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportfileCharacterEntryDbModels_SupportfileId",
                table: "SupportfileCharacterEntryDbModels",
                column: "SupportfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportfileEntryDbModels_SupportfileDbModels_SupportfileId",
                table: "SupportfileEntryDbModels",
                column: "SupportfileId",
                principalTable: "SupportfileDbModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportfileLogsDbModels_SupportfileDbModels_SupportfileId",
                table: "SupportfileLogsDbModels",
                column: "SupportfileId",
                principalTable: "SupportfileDbModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportfileEntryDbModels_SupportfileDbModels_SupportfileId",
                table: "SupportfileEntryDbModels");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportfileLogsDbModels_SupportfileDbModels_SupportfileId",
                table: "SupportfileLogsDbModels");

            migrationBuilder.DropTable(
                name: "SupportfileCharacterEntryDbModels");

            migrationBuilder.DropIndex(
                name: "IX_SupportfileLogsDbModels_SupportfileId",
                table: "SupportfileLogsDbModels");

            migrationBuilder.DropIndex(
                name: "IX_SupportfileEntryDbModels_SupportfileId",
                table: "SupportfileEntryDbModels");

            migrationBuilder.AlterColumn<string>(
                name: "SupportfileId",
                table: "SupportfileLogsDbModels",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SupportfileId",
                table: "SupportfileEntryDbModels",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccountApiBackupCopyDbModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DiscordId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastBackup = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SocialClubName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    State = table.Column<int>(type: "int", nullable: false),
                    StateReason = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountApiBackupCopyDbModels", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CharacterApiBackupCopyDbModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Cash = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Dimension = table.Column<int>(type: "int", nullable: false),
                    Energy = table.Column<double>(type: "double", nullable: false),
                    FirstName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gender = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Health = table.Column<double>(type: "double", nullable: false),
                    Hunger = table.Column<double>(type: "double", nullable: false),
                    LastBackup = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastLogout = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MiddleName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Position = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rotation = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Thirst = table.Column<double>(type: "double", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterApiBackupCopyDbModels", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SupportfileCharacterApiBackupCopy",
                columns: table => new
                {
                    CharacterApiBackupCopyId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SupportfileId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportfileCharacterApiBackupCopy", x => new { x.CharacterApiBackupCopyId, x.SupportfileId });
                    table.ForeignKey(
                        name: "FK_SupportfileCharacterApiBackupCopy_CharacterApiBackupCopyDbMo~",
                        column: x => x.CharacterApiBackupCopyId,
                        principalTable: "CharacterApiBackupCopyDbModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupportfileCharacterApiBackupCopy_SupportfileDbModels_Suppor~",
                        column: x => x.SupportfileId,
                        principalTable: "SupportfileDbModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SupportfileCharacterApiBackupCopy_SupportfileId",
                table: "SupportfileCharacterApiBackupCopy",
                column: "SupportfileId");
        }
    }
}
