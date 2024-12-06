using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sep.backend.v1.Migrations
{
    public partial class ModifyClassApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the foreign key constraint and the index for ClassApplicationCategoryId
            migrationBuilder.DropForeignKey(
                name: "FK_ClassApplication_ClassApplicationCategory_ClassApplicationC~",
                table: "ClassApplication");

            migrationBuilder.DropIndex(
                name: "IX_ClassApplication_ClassApplicationCategoryId",
                table: "ClassApplication");

            // Drop the ClassApplicationCategoryId column
            migrationBuilder.DropColumn(
                name: "ClassApplicationCategoryId",
                table: "ClassApplication");

            // Add foreign key constraint to ApplicationCategoryId
            migrationBuilder.AddForeignKey(
                name: "FK_ClassApplication_ApplicationCategory_ApplicationCategoryId",
                table: "ClassApplication",
                column: "ApplicationCategoryId",
                principalTable: "ClassApplicationCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Re-add the ClassApplicationCategoryId column
            migrationBuilder.AddColumn<int>(
                name: "ClassApplicationCategoryId",
                table: "ClassApplication",
                type: "integer",
                nullable: true);

            // Re-create the index for ClassApplicationCategoryId
            migrationBuilder.CreateIndex(
                name: "IX_ClassApplication_ClassApplicationCategoryId",
                table: "ClassApplication",
                column: "ClassApplicationCategoryId");

            // Re-add the foreign key constraint for ClassApplicationCategoryId
            migrationBuilder.AddForeignKey(
                name: "FK_ClassApplication_ClassApplicationCategory_ClassApplicationC~",
                table: "ClassApplication",
                column: "ClassApplicationCategoryId",
                principalTable: "ClassApplicationCategory",
                principalColumn: "Id");

            // Remove the foreign key constraint for ApplicationCategoryId
            migrationBuilder.DropForeignKey(
                name: "FK_ClassApplication_ApplicationCategory_ApplicationCategoryId",
                table: "ClassApplication");
        }
    }
}
