using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoiceVFileSystemBlazor.Migrations
{
    /// <inheritdoc />
    public partial class BugTrackerNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BugTrackerTaskCommentDbModels_BugTrackerTaskItemDbModels_Bug~",
                table: "BugTrackerTaskCommentDbModels");

            migrationBuilder.DropIndex(
                name: "IX_BugTrackerTaskCommentDbModels_BugTrackerTaskItemDbModelId",
                table: "BugTrackerTaskCommentDbModels");

            migrationBuilder.DropColumn(
                name: "BugTrackerTaskItemDbModelId",
                table: "BugTrackerTaskCommentDbModels");

            migrationBuilder.AddColumn<string>(
                name: "TaskId",
                table: "BugTrackerTaskCommentDbModels",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BugTrackerTaskCommentDbModels_TaskId",
                table: "BugTrackerTaskCommentDbModels",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_BugTrackerTaskCommentDbModels_BugTrackerTaskItemDbModels_Tas~",
                table: "BugTrackerTaskCommentDbModels",
                column: "TaskId",
                principalTable: "BugTrackerTaskItemDbModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BugTrackerTaskCommentDbModels_BugTrackerTaskItemDbModels_Tas~",
                table: "BugTrackerTaskCommentDbModels");

            migrationBuilder.DropIndex(
                name: "IX_BugTrackerTaskCommentDbModels_TaskId",
                table: "BugTrackerTaskCommentDbModels");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "BugTrackerTaskCommentDbModels");

            migrationBuilder.AddColumn<string>(
                name: "BugTrackerTaskItemDbModelId",
                table: "BugTrackerTaskCommentDbModels",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BugTrackerTaskCommentDbModels_BugTrackerTaskItemDbModelId",
                table: "BugTrackerTaskCommentDbModels",
                column: "BugTrackerTaskItemDbModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_BugTrackerTaskCommentDbModels_BugTrackerTaskItemDbModels_Bug~",
                table: "BugTrackerTaskCommentDbModels",
                column: "BugTrackerTaskItemDbModelId",
                principalTable: "BugTrackerTaskItemDbModels",
                principalColumn: "Id");
        }
    }
}
