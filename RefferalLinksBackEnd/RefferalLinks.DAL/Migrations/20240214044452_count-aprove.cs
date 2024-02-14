using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefferalLinks.DAL.Migrations
{
    public partial class countaprove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountAprove",
                table: "Customerlink",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountAprove",
                table: "Customerlink");
        }
    }
}
