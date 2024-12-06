using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sep.backend.v1.Migrations
{
    public partial class ModifyNotificationAndSchoolYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SchoolYearId",
                table: "Notifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SchoolYearId",
                table: "Notifications",
                column: "SchoolYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_SchoolYears_SchoolYearId",
                table: "Notifications",
                column: "SchoolYearId",
                principalTable: "SchoolYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_SchoolYears_SchoolYearId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_SchoolYearId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "SchoolYearId",
                table: "Notifications");
        }
    }
}
