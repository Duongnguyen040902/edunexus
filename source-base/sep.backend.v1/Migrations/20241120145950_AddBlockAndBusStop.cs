using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sep.backend.v1.Migrations
{
    public partial class AddBlockAndBusStop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Block",
                table: "Classes",
                type: "integer",
                nullable: true);
            
            migrationBuilder.AddColumn<int>(
                name: "BusStopId",
                table: "BusEnrollments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusEnrollments_BusStopId",
                table: "BusEnrollments",
                column: "BusStopId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusEnrollments_BusStops_BusStopId",
                table: "BusEnrollments",
                column: "BusStopId",
                principalTable: "BusStops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusEnrollments_BusStops_BusStopId",
                table: "BusEnrollments");

            migrationBuilder.DropIndex(
                name: "IX_BusEnrollments_BusStopId",
                table: "BusEnrollments");

            migrationBuilder.DropColumn(
                name: "Block",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "BusStopId",
                table: "BusEnrollments");
        }
    }
}
