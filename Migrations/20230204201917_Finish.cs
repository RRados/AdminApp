using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminApp.Migrations
{
    /// <inheritdoc />
    public partial class finish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogIn_User_UsernameIdUser",
                table: "LogIn");

            migrationBuilder.DropIndex(
                name: "IX_LogIn_UsernameIdUser",
                table: "LogIn");

            migrationBuilder.DropColumn(
                name: "UsernameIdUser",
                table: "LogIn");

            migrationBuilder.AddColumn<int>(
                name: "IdRoleNavigationIdRole",
                table: "LogIn",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "LogIn",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Username",
                table: "LogIn");

            migrationBuilder.AddColumn<int>(
                name: "UsernameIdUser",
                table: "LogIn",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LogIn_UsernameIdUser",
                table: "LogIn",
                column: "UsernameIdUser");

            migrationBuilder.AddForeignKey(
                name: "FK_LogIn_User_UsernameIdUser",
                table: "LogIn",
                column: "UsernameIdUser",
                principalTable: "User",
                principalColumn: "idUser",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
