using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaDeCalidad.API.Migrations
{
    /// <inheritdoc />
    public partial class MessagesTitleAndTheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Theme",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Theme",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Messages");
        }
    }
}
