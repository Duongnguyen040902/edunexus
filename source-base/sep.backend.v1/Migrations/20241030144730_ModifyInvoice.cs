using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sep.backend.v1.Migrations
{
    public partial class ModifyInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_SchoolSubscriptionPlans_SubscriptionPlanId",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "SubscriptionPlanId",
                table: "Invoices",
                newName: "SchoolSubscriptionPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_SubscriptionPlanId",
                table: "Invoices",
                newName: "IX_Invoices_SchoolSubscriptionPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_SchoolSubscriptionPlans_SchoolSubscriptionPlanId",
                table: "Invoices",
                column: "SchoolSubscriptionPlanId",
                principalTable: "SchoolSubscriptionPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_SchoolSubscriptionPlans_SchoolSubscriptionPlanId",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "SchoolSubscriptionPlanId",
                table: "Invoices",
                newName: "SubscriptionPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_SchoolSubscriptionPlanId",
                table: "Invoices",
                newName: "IX_Invoices_SubscriptionPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_SchoolSubscriptionPlans_SubscriptionPlanId",
                table: "Invoices",
                column: "SubscriptionPlanId",
                principalTable: "SchoolSubscriptionPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
