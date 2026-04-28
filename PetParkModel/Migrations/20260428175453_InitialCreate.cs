using System;
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
                name: "Owners",
                schema: "PetPark",
                columns: table => new
                {
                    OwnerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.OwnerId);
                });

            migrationBuilder.CreateTable(
                name: "Treats",
                schema: "PetPark",
                columns: table => new
                {
                    TreatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Calories = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treats", x => x.TreatId);
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
                name: "Pets",
                schema: "PetPark",
                columns: table => new
                {
                    PetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Species = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.PetId);
                    table.ForeignKey(
                        name: "FK_Pets_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "PetPark",
                        principalTable: "Owners",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PetTreats",
                schema: "PetPark",
                columns: table => new
                {
                    PetId = table.Column<int>(type: "int", nullable: false),
                    TreatId = table.Column<int>(type: "int", nullable: false),
                    DateGiven = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetTreats", x => new { x.PetId, x.TreatId });
                    table.ForeignKey(
                        name: "FK_PetTreats_Pets_PetId",
                        column: x => x.PetId,
                        principalSchema: "PetPark",
                        principalTable: "Pets",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PetTreats_Treats_TreatId",
                        column: x => x.TreatId,
                        principalSchema: "PetPark",
                        principalTable: "Treats",
                        principalColumn: "TreatId",
                        onDelete: ReferentialAction.Cascade);
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
                table: "Owners",
                columns: new[] { "OwnerId", "FullName" },
                values: new object[,]
                {
                    { 1, "Alex Carter" },
                    { 2, "Jordan Blake" },
                    { 3, "Taylor Morgan" }
                });

            migrationBuilder.InsertData(
                schema: "PetPark",
                table: "Treats",
                columns: new[] { "TreatId", "Calories", "Name" },
                values: new object[,]
                {
                    { 1, 40, "Peanut Biscuit" },
                    { 2, 30, "Salmon Bite" },
                    { 3, 10, "Carrot Chip" }
                });

            migrationBuilder.InsertData(
                schema: "PetPark",
                table: "Tricks",
                columns: new[] { "TrickId", "DifficultyLevel", "Name" },
                values: new object[,]
                {
                    { 1, "Easy", "Sit" },
                    { 2, "Medium", "Roll Over" },
                    { 10, "Easy", "Shake" },
                    { 11, "Hard", "Play Dead" },
                    { 12, "Medium", "Fetch" }
                });

            migrationBuilder.InsertData(
                schema: "PetPark",
                table: "Pets",
                columns: new[] { "PetId", "Name", "OwnerId", "Species" },
                values: new object[,]
                {
                    { 1, "Buddy", 1, "Dog" },
                    { 2, "Luna", 2, "Cat" },
                    { 10, "Mango", 1, "Parrot" },
                    { 11, "Shadow", 3, "Dog" },
                    { 12, "Cleo", 2, "Rabbit" }
                });

            migrationBuilder.InsertData(
                schema: "PetPark",
                table: "PetTreats",
                columns: new[] { "PetId", "TreatId", "DateGiven", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 2, 2, new DateTime(2026, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 10, 3, new DateTime(2026, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 11, 1, new DateTime(2026, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 12, 3, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 }
                });

            migrationBuilder.InsertData(
                schema: "PetPark",
                table: "PetTrick",
                columns: new[] { "PetsPetId", "TricksTrickId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 12 },
                    { 2, 1 },
                    { 2, 10 },
                    { 10, 10 },
                    { 11, 1 },
                    { 11, 11 },
                    { 11, 12 },
                    { 12, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pets_OwnerId",
                schema: "PetPark",
                table: "Pets",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PetTreats_TreatId",
                schema: "PetPark",
                table: "PetTreats",
                column: "TreatId");

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
                name: "PetTreats",
                schema: "PetPark");

            migrationBuilder.DropTable(
                name: "PetTrick",
                schema: "PetPark");

            migrationBuilder.DropTable(
                name: "Treats",
                schema: "PetPark");

            migrationBuilder.DropTable(
                name: "Pets",
                schema: "PetPark");

            migrationBuilder.DropTable(
                name: "Tricks",
                schema: "PetPark");

            migrationBuilder.DropTable(
                name: "Owners",
                schema: "PetPark");
        }
    }
}
