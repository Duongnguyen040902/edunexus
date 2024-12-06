using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sep.backend.v1.Migrations
{
    public partial class CRAddIsCompleteVerify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleteVerify",
                table: "Teachers",
                type: "BOOLEAN",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleteVerify",
                table: "BusSupervisors",
                type: "BOOLEAN",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleteVerify",
                table: "Pupils",
                type: "BOOLEAN",
                nullable: true,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleteVerify",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "IsCompleteVerify",
                table: "BusSupervisors");

            migrationBuilder.DropColumn(
                name: "IsCompleteVerify",
                table: "Pupils");
        }
    }
}
