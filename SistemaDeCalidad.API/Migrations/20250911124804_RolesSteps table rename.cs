using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaDeCalidad.API.Migrations
{
    /// <inheritdoc />
    public partial class RolesStepstablerename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolesStep_Roles_RoleId",
                table: "RolesStep");

            migrationBuilder.DropForeignKey(
                name: "FK_RolesStep_Steps_StepId",
                table: "RolesStep");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolesStep",
                table: "RolesStep");

            migrationBuilder.RenameTable(
                name: "RolesStep",
                newName: "RolesSteps");

            migrationBuilder.RenameIndex(
                name: "IX_RolesStep_StepId",
                table: "RolesSteps",
                newName: "IX_RolesSteps_StepId");

            migrationBuilder.RenameIndex(
                name: "IX_RolesStep_RoleId",
                table: "RolesSteps",
                newName: "IX_RolesSteps_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolesSteps",
                table: "RolesSteps",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolesSteps_Roles_RoleId",
                table: "RolesSteps",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolesSteps_Steps_StepId",
                table: "RolesSteps",
                column: "StepId",
                principalTable: "Steps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolesSteps_Roles_RoleId",
                table: "RolesSteps");

            migrationBuilder.DropForeignKey(
                name: "FK_RolesSteps_Steps_StepId",
                table: "RolesSteps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolesSteps",
                table: "RolesSteps");

            migrationBuilder.RenameTable(
                name: "RolesSteps",
                newName: "RolesStep");

            migrationBuilder.RenameIndex(
                name: "IX_RolesSteps_StepId",
                table: "RolesStep",
                newName: "IX_RolesStep_StepId");

            migrationBuilder.RenameIndex(
                name: "IX_RolesSteps_RoleId",
                table: "RolesStep",
                newName: "IX_RolesStep_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolesStep",
                table: "RolesStep",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolesStep_Roles_RoleId",
                table: "RolesStep",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolesStep_Steps_StepId",
                table: "RolesStep",
                column: "StepId",
                principalTable: "Steps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
