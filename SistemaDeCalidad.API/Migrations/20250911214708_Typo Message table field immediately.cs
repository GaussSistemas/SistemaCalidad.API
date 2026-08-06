using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaDeCalidad.API.Migrations
{
    /// <inheritdoc />
    public partial class TypoMessagetablefieldimmediately : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Inmediately",
                table: "Messages",
                newName: "Immediately");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Immediately",
                table: "Messages",
                newName: "Inmediately");
        }
    }
}
