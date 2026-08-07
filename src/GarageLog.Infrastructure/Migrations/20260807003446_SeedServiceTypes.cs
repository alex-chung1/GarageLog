using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GarageLog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedServiceTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "service_types",
                columns: new[] { "id", "category", "name" },
                values: new object[,]
                {
                    { 1, "Engine", "Oil Change" },
                    { 2, "Tires", "Tire Rotation" },
                    { 3, "Brakes", "Brake Pad Replacement" },
                    { 4, "Brakes", "Brake Fluid Flush" },
                    { 5, "Electrical", "Battery Replacement" },
                    { 6, "Engine", "Air Filter Replacement" },
                    { 7, "Interior", "Cabin Air Filter Replacement" },
                    { 8, "Cooling System", "Coolant Flush" },
                    { 9, "Transmission", "Transmission Service" },
                    { 10, "Engine", "Spark Plug Replacement" },
                    { 9999, "Other", "Other" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "service_types",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "service_types",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "service_types",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "service_types",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "service_types",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "service_types",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "service_types",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "service_types",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "service_types",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "service_types",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "service_types",
                keyColumn: "id",
                keyValue: 9999);
        }
    }
}
