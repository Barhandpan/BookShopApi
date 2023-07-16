using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace asp.networkshoprealapppublic.Migrations
{
    public partial class AddBookCoverPathColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookCoverPath",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookCoverPath",
                table: "Books");
        }
    }
}
