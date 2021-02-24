using Microsoft.EntityFrameworkCore.Migrations;

namespace Attendance50.Data.Migrations
{
    public partial class flows1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Checks");

            migrationBuilder.RenameColumn(
                name: "Flow",
                table: "Checks",
                newName: "FlowId");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Flows",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FlowId",
                table: "Checks",
                newName: "Flow");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Flows",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Checks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
