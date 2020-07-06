using Microsoft.EntityFrameworkCore.Migrations;

namespace TwitterBook.Data.Migrations
{
    public partial class droppeduserIdfrompostsagain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Posts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Posts",
                nullable: true);
        }
    }
}
