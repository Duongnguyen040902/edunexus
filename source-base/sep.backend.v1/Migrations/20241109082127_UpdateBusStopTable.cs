using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sep.backend.v1.Migrations
{
    public partial class UpdateBusStopTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {    
            migrationBuilder.RenameColumn(
                name: "EstimatedTime",
                table: "BusStops",
                newName: "ReturnTime");   

            migrationBuilder.AddColumn<TimeSpan>(
                name: "PickUpTime",
                table: "BusStops",
                type: "time(7) without time zone",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {     
            migrationBuilder.DropColumn(
                name: "PickUpTime",
                table: "BusStops");

            migrationBuilder.RenameColumn(
                name: "ReturnTime",
                table: "BusStops",
                newName: "EstimatedTime");
        }
    }
}
