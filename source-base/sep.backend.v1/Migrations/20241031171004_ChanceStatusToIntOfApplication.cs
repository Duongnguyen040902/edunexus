using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sep.backend.v1.Migrations
{
    public partial class ChanceStatusToIntOfApplication : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"ClassApplication\" ALTER COLUMN \"Status\" TYPE integer USING \"Status\"::integer;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"ClassApplication\" ALTER COLUMN \"Status\" TYPE text USING \"Status\"::text;");
        }
    }
}
