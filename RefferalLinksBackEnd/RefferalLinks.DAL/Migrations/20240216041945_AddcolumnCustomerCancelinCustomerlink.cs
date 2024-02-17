using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefferalLinks.DAL.Migrations
{
    public partial class AddcolumnCustomerCancelinCustomerlink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CustomerCancel",
                table: "Customerlink",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerCancel",
                table: "Customerlink");
        }
    }
}
