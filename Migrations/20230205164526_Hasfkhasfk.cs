using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminApp.Migrations
{
    /// <inheritdoc />
    public partial class hasfkhasfk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Role",
                table: "UserRole");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "LogIn",
                newName: "Email");

            migrationBuilder.AddColumn<string>(
                name: "ConfirmPassword",
                table: "LogIn",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Role",
                table: "UserRole",
                column: "idRole",
                principalTable: "Role",
                principalColumn: "idRole");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Role",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "ConfirmPassword",
                table: "LogIn");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "LogIn",
                newName: "Username");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Role",
                table: "UserRole",
                column: "idRole",
                principalTable: "Role",
                principalColumn: "idRole",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
