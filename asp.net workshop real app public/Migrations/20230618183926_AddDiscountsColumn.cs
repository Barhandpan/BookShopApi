using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace asp.networkshoprealapppublic.Migrations
{
    public partial class AddDiscountsColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Discounts",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discounts",
                table: "AspNetUsers");
        }
    }
}