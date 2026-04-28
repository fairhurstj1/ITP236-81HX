using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PetParkModel.Migrations
{
    /// <inheritdoc />
    public partial class SeedExpansion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "PetPark",
                table: "Pets",
                columns: new[] { "PetId", "Name", "Species" },
                values: new object[,]
                {
                    { 10, "Mango", "Parrot" },
                    { 11, "Shadow", "Dog" },
                    { 12, "Cleo", "Rabbit" }
                });

            migrationBuilder.InsertData(
                schema: "PetPark",
                table: "Tricks",
                columns: new[] { "TrickId", "DifficultyLevel", "Name" },
                values: new object[,]
                {
                    { 10, "Easy", "Shake" },
                    { 11, "Hard", "Play Dead" },
                    { 12, "Medium", "Fetch" }
                });

            migrationBuilder.InsertData(
                schema: "PetPark",
                table: "PetTrick",
                columns: new[] { "PetsPetId", "TricksTrickId" },
                values: new object[,]
                {
                    { 1, 12 },
                    { 2, 10 },
                    { 10, 10 },
                    { 11, 1 },
                    { 11, 11 },
                    { 11, 12 },
                    { 12, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "PetPark",
                table: "PetTrick",
                keyColumns: new[] { "PetsPetId", "TricksTrickId" },
                keyValues: new object[] { 1, 12 });

            migrationBuilder.DeleteData(
                schema: "PetPark",
                table: "PetTrick",
                keyColumns: new[] { "PetsPetId", "TricksTrickId" },
                keyValues: new object[] { 2, 10 });

            migrationBuilder.DeleteData(
                schema: "PetPark",
                table: "PetTrick",
                keyColumns: new[] { "PetsPetId", "TricksTrickId" },
                keyValues: new object[] { 10, 10 });

            migrationBuilder.DeleteData(
                schema: "PetPark",
                table: "PetTrick",
                keyColumns: new[] { "PetsPetId", "TricksTrickId" },
                keyValues: new object[] { 11, 1 });

            migrationBuilder.DeleteData(
                schema: "PetPark",
                table: "PetTrick",
                keyColumns: new[] { "PetsPetId", "TricksTrickId" },
                keyValues: new object[] { 11, 11 });

            migrationBuilder.DeleteData(
                schema: "PetPark",
                table: "PetTrick",
                keyColumns: new[] { "PetsPetId", "TricksTrickId" },
                keyValues: new object[] { 11, 12 });

            migrationBuilder.DeleteData(
                schema: "PetPark",
                table: "PetTrick",
                keyColumns: new[] { "PetsPetId", "TricksTrickId" },
                keyValues: new object[] { 12, 2 });

            migrationBuilder.DeleteData(
                schema: "PetPark",
                table: "Pets",
                keyColumn: "PetId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "PetPark",
                table: "Pets",
                keyColumn: "PetId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "PetPark",
                table: "Pets",
                keyColumn: "PetId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "PetPark",
                table: "Tricks",
                keyColumn: "TrickId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "PetPark",
                table: "Tricks",
                keyColumn: "TrickId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "PetPark",
                table: "Tricks",
                keyColumn: "TrickId",
                keyValue: 12);
        }
    }
}
