using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageLog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleDetailsAndServiceChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "current_mileage",
                table: "vehicles");

            migrationBuilder.DropColumn(
                name: "is_custom_entry",
                table: "service_types");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "service_record_items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "current_mileage",
                table: "vehicles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_custom_entry",
                table: "service_types",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "service_record_items",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
