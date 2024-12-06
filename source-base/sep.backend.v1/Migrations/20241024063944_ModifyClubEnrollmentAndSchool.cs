using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sep.backend.v1.Migrations
{
    public partial class ModifyClubEnrollmentAndSchool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClubEnrollments_PupilId_SemesterId",
                table: "ClubEnrollments");

            migrationBuilder.DropIndex(
                name: "IX_ClubEnrollments_TeacherId_SemesterId",
                table: "ClubEnrollments");

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Schools",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Schools",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ward",
                table: "Schools",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the added columns from the Schools table
            migrationBuilder.DropColumn(
                name: "District",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "Ward",
                table: "Schools");

            migrationBuilder.CreateIndex(
                name: "IX_ClubEnrollments_PupilId_SemesterId",
                table: "ClubEnrollments",
                columns: new[] { "PupilId", "SemesterId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClubEnrollments_TeacherId_SemesterId",
                table: "ClubEnrollments",
                columns: new[] { "TeacherId", "SemesterId" });
        }

    }
}
