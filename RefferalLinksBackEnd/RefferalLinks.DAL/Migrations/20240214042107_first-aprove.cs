using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefferalLinks.DAL.Migrations
{
    public partial class firstaprove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AproveFirst",
                table: "Customerlink",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AproveFirst",
                table: "Customerlink");
        }
    }
}
