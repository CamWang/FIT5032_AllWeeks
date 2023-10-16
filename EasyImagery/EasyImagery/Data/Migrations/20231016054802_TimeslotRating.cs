using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyImagery.Data.Migrations
{
    public partial class TimeslotRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Timeslot",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Timeslot");
        }
    }
}
