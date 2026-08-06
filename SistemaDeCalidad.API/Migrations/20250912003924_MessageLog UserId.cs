using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaDeCalidad.API.Migrations
{
    /// <inheritdoc />
    public partial class MessageLogUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EiffelUserId",
                table: "MessagesLogs");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "MessagesLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MessagesLogs");

            migrationBuilder.AddColumn<string>(
                name: "EiffelUserId",
                table: "MessagesLogs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
