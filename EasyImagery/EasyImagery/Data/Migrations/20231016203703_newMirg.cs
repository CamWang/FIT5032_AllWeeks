using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyImagery.Data.Migrations
{
    public partial class newMirg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Timeslot_TimeslotId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TimeslotId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "PatientId",
                table: "Timeslot",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Timeslot_PatientId",
                table: "Timeslot",
                column: "PatientId",
                unique: true,
                filter: "[PatientId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Timeslot_AspNetUsers_PatientId",
                table: "Timeslot",
                column: "PatientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timeslot_AspNetUsers_PatientId",
                table: "Timeslot");

            migrationBuilder.DropIndex(
                name: "IX_Timeslot_PatientId",
                table: "Timeslot");

            migrationBuilder.AlterColumn<string>(
                name: "PatientId",
                table: "Timeslot",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TimeslotId",
                table: "AspNetUsers",
                column: "TimeslotId",
                unique: true,
                filter: "[TimeslotId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Timeslot_TimeslotId",
                table: "AspNetUsers",
                column: "TimeslotId",
                principalTable: "Timeslot",
                principalColumn: "Id");
        }
    }
}
