using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminApp.Migrations
{
    /// <inheritdoc />
    public partial class LogReg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogIn_Role_IdRoleNavigationIdRole",
                table: "LogIn");

            migrationBuilder.DropIndex(
                name: "IX_LogIn_IdRoleNavigationIdRole",
                table: "LogIn");

            migrationBuilder.DropColumn(
                name: "IdRoleNavigationIdRole",
                table: "LogIn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdRoleNavigationIdRole",
                table: "LogIn",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LogIn_IdRoleNavigationIdRole",
                table: "LogIn",
                column: "IdRoleNavigationIdRole");

            migrationBuilder.AddForeignKey(
                name: "FK_LogIn_Role_IdRoleNavigationIdRole",
                table: "LogIn",
                column: "IdRoleNavigationIdRole",
                principalTable: "Role",
                principalColumn: "idRole");
        }
    }
}
