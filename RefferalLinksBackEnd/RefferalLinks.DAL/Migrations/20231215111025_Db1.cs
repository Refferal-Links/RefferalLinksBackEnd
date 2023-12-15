using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefferalLinks.DAL.Migrations
{
    public partial class Db1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DateOfBirth",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Job",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldPassport",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Job",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "OldPassport",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Customer");
        }
    }
}
