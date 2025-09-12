using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SistemaDeCalidad.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveunnecesarytableMessagesSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessagesSettings");

            migrationBuilder.AddColumn<string>(
                name: "Template",
                table: "MessagesTypes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Template",
                table: "MessagesTypes");

            migrationBuilder.CreateTable(
                name: "MessagesSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MessageTypeId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MessageText = table.Column<string>(type: "text", nullable: false),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagesSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessagesSettings_MessagesTypes_MessageTypeId",
                        column: x => x.MessageTypeId,
                        principalTable: "MessagesTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessagesSettings_MessageTypeId",
                table: "MessagesSettings",
                column: "MessageTypeId",
                unique: true);
        }
    }
}
