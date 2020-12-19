using Microsoft.EntityFrameworkCore.Migrations;

namespace Attendance.Migrations
{
    public partial class renameMarks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marks_Meets_MeetId",
                table: "Marks");

            migrationBuilder.DropForeignKey(
                name: "FK_Marks_Students_StudentId",
                table: "Marks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Marks",
                table: "Marks");

            migrationBuilder.RenameTable(
                name: "Marks",
                newName: "MeetStudents");

            migrationBuilder.RenameIndex(
                name: "IX_Marks_StudentId",
                table: "MeetStudents",
                newName: "IX_MeetStudents_StudentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeetStudents",
                table: "MeetStudents",
                columns: new[] { "MeetId", "StudentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MeetStudents_Meets_MeetId",
                table: "MeetStudents",
                column: "MeetId",
                principalTable: "Meets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MeetStudents_Students_StudentId",
                table: "MeetStudents",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetStudents_Meets_MeetId",
                table: "MeetStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_MeetStudents_Students_StudentId",
                table: "MeetStudents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeetStudents",
                table: "MeetStudents");

            migrationBuilder.RenameTable(
                name: "MeetStudents",
                newName: "Marks");

            migrationBuilder.RenameIndex(
                name: "IX_MeetStudents_StudentId",
                table: "Marks",
                newName: "IX_Marks_StudentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Marks",
                table: "Marks",
                columns: new[] { "MeetId", "StudentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_Meets_MeetId",
                table: "Marks",
                column: "MeetId",
                principalTable: "Meets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_Students_StudentId",
                table: "Marks",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
