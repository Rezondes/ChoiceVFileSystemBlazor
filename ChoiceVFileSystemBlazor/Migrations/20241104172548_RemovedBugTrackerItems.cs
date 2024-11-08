using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoiceVFileSystemBlazor.Migrations
{
    /// <inheritdoc />
    public partial class RemovedBugTrackerItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BugTrackerTaskCommentDbModels");

            migrationBuilder.DropTable(
                name: "BugTrackerTaskItemDbModels");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BugTrackerTaskItemDbModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Category = table.Column<int>(type: "int", nullable: false),
                    CreatedByDiscordId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedByDiscordName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BugTrackerTaskItemDbModels", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BugTrackerTaskCommentDbModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TaskId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DiscordId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DiscordName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Message = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BugTrackerTaskCommentDbModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BugTrackerTaskCommentDbModels_BugTrackerTaskItemDbModels_Tas~",
                        column: x => x.TaskId,
                        principalTable: "BugTrackerTaskItemDbModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BugTrackerTaskCommentDbModels_TaskId",
                table: "BugTrackerTaskCommentDbModels",
                column: "TaskId");
        }
    }
}
