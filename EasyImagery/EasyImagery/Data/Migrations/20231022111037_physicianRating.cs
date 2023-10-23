using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyImagery.Data.Migrations
{
    public partial class physicianRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhysicianRating",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhysicianRating",
                table: "AspNetUsers");
        }
    }
}
