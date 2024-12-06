using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sep.backend.v1.Migrations
{
    public partial class AddIndexToBusStop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "BusStops",
                type: "integer",
                nullable: false,
                defaultValue: 0);          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {          
            migrationBuilder.DropColumn(
                name: "Index",
                table: "BusStops");         
        }
    }
}
