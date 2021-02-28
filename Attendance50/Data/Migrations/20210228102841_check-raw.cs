using Microsoft.EntityFrameworkCore.Migrations;

namespace Attendance50.Data.Migrations
{
    public partial class checkraw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Raw",
                table: "Checks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Raw",
                table: "Checks");
        }
    }
}
