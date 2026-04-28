using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PetParkModel.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "PetPark");

            migrationBuilder.CreateTable(
                name: "Pets",
                schema: "PetPark",
                columns: table => new
                {
                    PetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Species = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.PetId);
                });

            migrationBuilder.CreateTable(
                name: "Tricks",
                schema: "PetPark",
                columns: table => new
                {
                    TrickId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DifficultyLevel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tricks", x => x.TrickId);
                });

            migrationBuilder.CreateTable(
                name: "PetTrick",
                schema: "PetPark",
                columns: table => new
                {
                    PetsPetId = table.Column<int>(type: "int", nullable: false),
                    TricksTrickId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetTrick", x => new { x.PetsPetId, x.TricksTrickId });
                    table.ForeignKey(
                        name: "FK_PetTrick_Pets_PetsPetId",
                        column: x => x.PetsPetId,
                        principalSchema: "PetPark",
                        principalTable: "Pets",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PetTrick_Tricks_TricksTrickId",
                        column: x => x.TricksTrickId,
                        principalSchema: "PetPark",
                        principalTable: "Tricks",
                        principalColumn: "TrickId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "PetPark",
                table: "Pets",
                columns: new[] { "PetId", "Name", "Species" },
                values: new object[,]
                {
                    { 1, "Buddy", "Dog" },
                    { 2, "Luna", "Cat" }
                });

            migrationBuilder.InsertData(
                schema: "PetPark",
                table: "Tricks",
                columns: new[] { "TrickId", "DifficultyLevel", "Name" },
                values: new object[,]
                {
                    { 1, "Easy", "Sit" },
                    { 2, "Medium", "Roll Over" }
                });

            migrationBuilder.InsertData(
                schema: "PetPark",
                table: "PetTrick",
                columns: new[] { "PetsPetId", "TricksTrickId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PetTrick_TricksTrickId",
                schema: "PetPark",
                table: "PetTrick",
                column: "TricksTrickId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PetTrick",
                schema: "PetPark");

            migrationBuilder.DropTable(
                name: "Pets",
                schema: "PetPark");

            migrationBuilder.DropTable(
                name: "Tricks",
                schema: "PetPark");
        }
    }
}
