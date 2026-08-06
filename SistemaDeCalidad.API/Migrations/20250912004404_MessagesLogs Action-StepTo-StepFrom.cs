using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaDeCalidad.API.Migrations
{
    /// <inheritdoc />
    public partial class MessagesLogsActionStepToStepFrom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "MessagesLogs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StepFrom",
                table: "MessagesLogs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StepTo",
                table: "MessagesLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MessagesLogs_UserId",
                table: "MessagesLogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesLogs_Users_UserId",
                table: "MessagesLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessagesLogs_Users_UserId",
                table: "MessagesLogs");

            migrationBuilder.DropIndex(
                name: "IX_MessagesLogs_UserId",
                table: "MessagesLogs");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "MessagesLogs");

            migrationBuilder.DropColumn(
                name: "StepFrom",
                table: "MessagesLogs");

            migrationBuilder.DropColumn(
                name: "StepTo",
                table: "MessagesLogs");
        }
    }
}
