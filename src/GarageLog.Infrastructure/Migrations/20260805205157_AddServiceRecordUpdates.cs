using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageLog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceRecordUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_service_records_vehicles_vehicle_id",
                table: "service_records");

            migrationBuilder.AddForeignKey(
                name: "fk_service_records_vehicles_vehicle_id",
                table: "service_records",
                column: "vehicle_id",
                principalTable: "vehicles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_service_records_vehicles_vehicle_id",
                table: "service_records");

            migrationBuilder.AddForeignKey(
                name: "fk_service_records_vehicles_vehicle_id",
                table: "service_records",
                column: "vehicle_id",
                principalTable: "vehicles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
