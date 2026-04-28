using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolModel.Migrations
{
    /// <inheritdoc />
    public partial class InitialGenerate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "School");

            migrationBuilder.CreateTable(
                name: "Course",
                schema: "School",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Credits = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.CourseId);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                schema: "School",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Major = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.StudentId);
                });

            migrationBuilder.CreateTable(
                name: "Enrollment",
                schema: "School",
                columns: table => new
                {
                    EnrollmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollment", x => x.EnrollmentId);
                    table.ForeignKey(
                        name: "FK_Enrollment_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "School",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollment_Student_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "School",
                        principalTable: "Student",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "School",
                table: "Course",
                columns: new[] { "CourseId", "Credits", "Tag", "Title" },
                values: new object[,]
                {
                    { 201, 4, "ISP-136", "Introduction to C#" },
                    { 202, 3, "MTH-200", "Discrete Math" }
                });

            migrationBuilder.InsertData(
                schema: "School",
                table: "Student",
                columns: new[] { "StudentId", "FirstName", "LastName", "Major" },
                values: new object[,]
                {
                    { 101, "Ada", "Lovelace", "Math" },
                    { 102, "Grace", "Hopper", "ITP" }
                });

            migrationBuilder.InsertData(
                schema: "School",
                table: "Enrollment",
                columns: new[] { "EnrollmentId", "CourseId", "StudentId" },
                values: new object[,]
                {
                    { 301, 201, 101 },
                    { 302, 202, 102 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_CourseId",
                schema: "School",
                table: "Enrollment",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_StudentId",
                schema: "School",
                table: "Enrollment",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollment",
                schema: "School");

            migrationBuilder.DropTable(
                name: "Course",
                schema: "School");

            migrationBuilder.DropTable(
                name: "Student",
                schema: "School");
        }
    }
}