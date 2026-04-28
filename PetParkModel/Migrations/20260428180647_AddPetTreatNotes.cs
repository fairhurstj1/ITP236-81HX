using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetParkModel.Migrations
{
    /// <inheritdoc />
    public partial class AddPetTreatNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "PetPark",
                table: "PetTreats",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "PetPark",
                table: "PetTreats",
                keyColumns: new[] { "PetId", "TreatId" },
                keyValues: new object[] { 1, 1 },
                column: "Notes",
                value: null);

            migrationBuilder.UpdateData(
                schema: "PetPark",
                table: "PetTreats",
                keyColumns: new[] { "PetId", "TreatId" },
                keyValues: new object[] { 2, 2 },
                column: "Notes",
                value: null);

            migrationBuilder.UpdateData(
                schema: "PetPark",
                table: "PetTreats",
                keyColumns: new[] { "PetId", "TreatId" },
                keyValues: new object[] { 10, 3 },
                column: "Notes",
                value: null);

            migrationBuilder.UpdateData(
                schema: "PetPark",
                table: "PetTreats",
                keyColumns: new[] { "PetId", "TreatId" },
                keyValues: new object[] { 11, 1 },
                column: "Notes",
                value: null);

            migrationBuilder.UpdateData(
                schema: "PetPark",
                table: "PetTreats",
                keyColumns: new[] { "PetId", "TreatId" },
                keyValues: new object[] { 12, 3 },
                column: "Notes",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "PetPark",
                table: "PetTreats");
        }
    }
}
