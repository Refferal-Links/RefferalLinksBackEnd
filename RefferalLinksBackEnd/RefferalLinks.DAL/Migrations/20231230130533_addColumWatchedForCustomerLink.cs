using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefferalLinks.DAL.Migrations
{
    public partial class addColumWatchedForCustomerLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Watched",
                table: "Customerlink",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Watched",
                table: "Customerlink");
        }
    }
}
