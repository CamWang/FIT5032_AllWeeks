using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyImagery.Data.Migrations
{
    public partial class mergeIdentities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Clinic_ClinicId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Clinic_Manager_ClinicId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClinicId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Manager_ClinicId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Manager_Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Patient_Name",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "TimeslotId",
                table: "AspNetUsers",
                newName: "PatientTimeslotId");

            migrationBuilder.RenameColumn(
                name: "Manager_ClinicId",
                table: "AspNetUsers",
                newName: "PhysicianClinicId");

            migrationBuilder.RenameColumn(
                name: "ClinicId",
                table: "AspNetUsers",
                newName: "ManagerClinicId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ManagerClinicId",
                table: "AspNetUsers",
                column: "ManagerClinicId",
                unique: true,
                filter: "[ManagerClinicId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PhysicianClinicId",
                table: "AspNetUsers",
                column: "PhysicianClinicId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Clinic_ManagerClinicId",
                table: "AspNetUsers",
                column: "ManagerClinicId",
                principalTable: "Clinic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Clinic_PhysicianClinicId",
                table: "AspNetUsers",
                column: "PhysicianClinicId",
                principalTable: "Clinic",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Clinic_ManagerClinicId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Clinic_PhysicianClinicId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ManagerClinicId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PhysicianClinicId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "PhysicianClinicId",
                table: "AspNetUsers",
                newName: "Manager_ClinicId");

            migrationBuilder.RenameColumn(
                name: "PatientTimeslotId",
                table: "AspNetUsers",
                newName: "TimeslotId");

            migrationBuilder.RenameColumn(
                name: "ManagerClinicId",
                table: "AspNetUsers",
                newName: "ClinicId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Manager_Name",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Patient_Name",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClinicId",
                table: "AspNetUsers",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Manager_ClinicId",
                table: "AspNetUsers",
                column: "Manager_ClinicId",
                unique: true,
                filter: "[Manager_ClinicId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Clinic_ClinicId",
                table: "AspNetUsers",
                column: "ClinicId",
                principalTable: "Clinic",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Clinic_Manager_ClinicId",
                table: "AspNetUsers",
                column: "Manager_ClinicId",
                principalTable: "Clinic",
                principalColumn: "Id");
        }
    }
}
