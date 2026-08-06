using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaDeCalidad.API.Migrations
{
    /// <inheritdoc />
    public partial class StepsSistemBlocked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SistemBlocked",
                table: "Steps",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SistemBlocked",
                table: "Steps");
        }
    }
}
