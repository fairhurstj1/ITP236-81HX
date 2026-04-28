using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolModel.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "School",
                table: "Student",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "School",
                table: "Student",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grade",
                schema: "School",
                table: "Enrollment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                schema: "School",
                table: "Course",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Club",
                schema: "School",
                columns: table => new
                {
                    ClubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Club", x => x.ClubId);
                });

            migrationBuilder.CreateTable(
                name: "Teacher",
                schema: "School",
                columns: table => new
                {
                    TeacherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.TeacherId);
                });

            migrationBuilder.CreateTable(
                name: "ClubStudent",
                schema: "School",
                columns: table => new
                {
                    ClubsClubId = table.Column<int>(type: "int", nullable: false),
                    MembersStudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubStudent", x => new { x.ClubsClubId, x.MembersStudentId });
                    table.ForeignKey(
                        name: "FK_ClubStudent_Club_ClubsClubId",
                        column: x => x.ClubsClubId,
                        principalSchema: "School",
                        principalTable: "Club",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubStudent_Student_MembersStudentId",
                        column: x => x.MembersStudentId,
                        principalSchema: "School",
                        principalTable: "Student",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "School",
                table: "Club",
                columns: new[] { "ClubId", "Name" },
                values: new object[] { 101, "STEM" });

            migrationBuilder.UpdateData(
                schema: "School",
                table: "Course",
                keyColumn: "CourseId",
                keyValue: 201,
                column: "TeacherId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "School",
                table: "Course",
                keyColumn: "CourseId",
                keyValue: 202,
                column: "TeacherId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "School",
                table: "Enrollment",
                keyColumn: "EnrollmentId",
                keyValue: 301,
                column: "Grade",
                value: null);

            migrationBuilder.UpdateData(
                schema: "School",
                table: "Enrollment",
                keyColumn: "EnrollmentId",
                keyValue: 302,
                column: "Grade",
                value: null);

            migrationBuilder.UpdateData(
                schema: "School",
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 101,
                columns: new[] { "Email", "Major", "PhoneNumber" },
                values: new object[] { "ALovelace@reynolds.edu", "Mathematics", "804-345-6789" });

            migrationBuilder.UpdateData(
                schema: "School",
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 102,
                columns: new[] { "Email", "PhoneNumber" },
                values: new object[] { "GHopper@reynolds.edu", "804-987-6543" });

            migrationBuilder.InsertData(
                schema: "School",
                table: "Teacher",
                columns: new[] { "TeacherId", "FullName" },
                values: new object[] { 101, "Bob Dust" });

            migrationBuilder.CreateIndex(
                name: "IX_Course_TeacherId",
                schema: "School",
                table: "Course",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubStudent_MembersStudentId",
                schema: "School",
                table: "ClubStudent",
                column: "MembersStudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Teacher_TeacherId",
                schema: "School",
                table: "Course",
                column: "TeacherId",
                principalSchema: "School",
                principalTable: "Teacher",
                principalColumn: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Teacher_TeacherId",
                schema: "School",
                table: "Course");

            migrationBuilder.DropTable(
                name: "ClubStudent",
                schema: "School");

            migrationBuilder.DropTable(
                name: "Teacher",
                schema: "School");

            migrationBuilder.DropTable(
                name: "Club",
                schema: "School");

            migrationBuilder.DropIndex(
                name: "IX_Course_TeacherId",
                schema: "School",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "School",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "School",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "Grade",
                schema: "School",
                table: "Enrollment");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                schema: "School",
                table: "Course");

            migrationBuilder.UpdateData(
                schema: "School",
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 101,
                column: "Major",
                value: "Math");
        }
    }
}
