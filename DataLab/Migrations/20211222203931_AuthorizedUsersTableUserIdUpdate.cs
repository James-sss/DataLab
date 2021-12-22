using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLab.Migrations
{
    public partial class AuthorizedUsersTableUserIdUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizedUsers_AspNetUsers_UserId",
                table: "AuthorizedUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthorizedUsers",
                table: "AuthorizedUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AuthorizedUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthorizedUsers",
                table: "AuthorizedUsers",
                columns: new[] { "CustomerId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizedUsers_AspNetUsers_UserId",
                table: "AuthorizedUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizedUsers_AspNetUsers_UserId",
                table: "AuthorizedUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthorizedUsers",
                table: "AuthorizedUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AuthorizedUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthorizedUsers",
                table: "AuthorizedUsers",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizedUsers_AspNetUsers_UserId",
                table: "AuthorizedUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
