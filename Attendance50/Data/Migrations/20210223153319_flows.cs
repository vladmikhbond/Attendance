using Microsoft.EntityFrameworkCore.Migrations;

namespace Attendance50.Data.Migrations
{
    public partial class flows : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlowStudents_Flow_FlowId",
                table: "FlowStudents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Flow",
                table: "Flow");

            migrationBuilder.RenameTable(
                name: "Flow",
                newName: "Flows");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flows",
                table: "Flows",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FlowStudents_Flows_FlowId",
                table: "FlowStudents",
                column: "FlowId",
                principalTable: "Flows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlowStudents_Flows_FlowId",
                table: "FlowStudents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Flows",
                table: "Flows");

            migrationBuilder.RenameTable(
                name: "Flows",
                newName: "Flow");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flow",
                table: "Flow",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FlowStudents_Flow_FlowId",
                table: "FlowStudents",
                column: "FlowId",
                principalTable: "Flow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
