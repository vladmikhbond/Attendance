using Microsoft.EntityFrameworkCore.Migrations;

namespace Attendance50.Data.Migrations
{
    public partial class flows2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Checks_FlowId",
                table: "Checks",
                column: "FlowId");

            migrationBuilder.AddForeignKey(
                name: "FK_Checks_Flows_FlowId",
                table: "Checks",
                column: "FlowId",
                principalTable: "Flows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checks_Flows_FlowId",
                table: "Checks");

            migrationBuilder.DropIndex(
                name: "IX_Checks_FlowId",
                table: "Checks");
        }
    }
}
