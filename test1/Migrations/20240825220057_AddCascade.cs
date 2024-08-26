using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace test1.Migrations
{
    /// <inheritdoc />
    public partial class AddCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerRoles_Role_RoleId",
                table: "CustomerRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRoles_Role_RoleId",
                table: "CustomerRoles",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerRoles_Role_RoleId",
                table: "CustomerRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRoles_Role_RoleId",
                table: "CustomerRoles",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
