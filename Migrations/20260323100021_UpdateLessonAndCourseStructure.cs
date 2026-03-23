using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodingCourses.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLessonAndCourseStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Lessons");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Lessons",
                newName: "VideoUrl");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Lessons");

            migrationBuilder.RenameColumn(
                name: "VideoUrl",
                table: "Lessons",
                newName: "Content");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Lessons",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
