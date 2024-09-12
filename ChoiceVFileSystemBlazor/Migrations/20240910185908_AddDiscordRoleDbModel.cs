using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoiceVFileSystemBlazor.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscordRoleDbModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Supportfiles",
                table: "Supportfiles");

            migrationBuilder.RenameTable(
                name: "Supportfiles",
                newName: "SupportfileDbModels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupportfileDbModels",
                table: "SupportfileDbModels",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DiscordRoleDbModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DiscordRoleId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    AutomaticRank = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordRoleDbModels", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DiscordRoleLogsDbModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    AccessId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordRoleLogsDbModels", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscordRoleDbModels");

            migrationBuilder.DropTable(
                name: "DiscordRoleLogsDbModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupportfileDbModels",
                table: "SupportfileDbModels");

            migrationBuilder.RenameTable(
                name: "SupportfileDbModels",
                newName: "Supportfiles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Supportfiles",
                table: "Supportfiles",
                column: "Id");
        }
    }
}
