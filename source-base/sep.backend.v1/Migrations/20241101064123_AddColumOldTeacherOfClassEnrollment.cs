using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sep.backend.v1.Migrations
{
    public partial class AddColumOldTeacherOfClassEnrollment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {   
            migrationBuilder.AddColumn<string>(
                name: "OldTeacher",
                table: "ClassEnrollments",
                type: "text",
                nullable: true); 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldTeacher",
                table: "ClassEnrollments");    
        }
    }
}
