using Microsoft.EntityFrameworkCore.Migrations;

namespace sep.backend.v1.Migrations
{
    public partial class AddMaxActiveAccountAndModifyBlackList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxActiveAccounts",
                table: "SubscriptionPlans",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "AccessToken",
                table: "Blacklists",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "Blacklists",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxActiveAccounts",
                table: "SubscriptionPlans");

            migrationBuilder.AlterColumn<string>(
                name: "AccessToken",
                table: "Blacklists",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "Blacklists",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}