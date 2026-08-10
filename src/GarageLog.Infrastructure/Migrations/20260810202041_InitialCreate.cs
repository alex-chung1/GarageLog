using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GarageLog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "identity");

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "identity",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "service_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    category = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "identity",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role_claims",
                schema: "identity",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_role_claims_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "identity",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_claims",
                schema: "identity",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_claims_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_logins",
                schema: "identity",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    provider_key = table.Column<string>(type: "text", nullable: false),
                    provider_display_name = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_logins", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_user_logins_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                schema: "identity",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "identity",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_tokens",
                schema: "identity",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_user_tokens_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    make = table.Column<string>(type: "text", nullable: false),
                    model = table.Column<string>(type: "text", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    vin = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicles", x => x.id);
                    table.ForeignKey(
                        name: "fk_vehicles_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_records",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vehicle_id = table.Column<int>(type: "integer", nullable: false),
                    service_date = table.Column<DateOnly>(type: "date", nullable: false),
                    mileage = table.Column<int>(type: "integer", nullable: false),
                    is_self_service = table.Column<bool>(type: "boolean", nullable: false),
                    shop_name = table.Column<string>(type: "text", nullable: true),
                    total_cost = table.Column<decimal>(type: "numeric", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_records", x => x.id);
                    table.ForeignKey(
                        name: "fk_service_records_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_record_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    service_record_id = table.Column<int>(type: "integer", nullable: false),
                    service_type_id = table.Column<int>(type: "integer", nullable: false),
                    custom_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_record_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_service_record_items_service_records_service_record_id",
                        column: x => x.service_record_id,
                        principalTable: "service_records",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_service_record_items_service_types_service_type_id",
                        column: x => x.service_type_id,
                        principalTable: "service_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "service_types",
                columns: new[] { "id", "category", "name" },
                values: new object[,]
                {
                    { 1, "Engine", "Oil Change" },
                    { 2, "Tires", "Tire Rotation" },
                    { 3, "Brakes", "Brake Pad Replacement" },
                    { 1000, "Body & Frame", "Body Inspected" },
                    { 1001, "Body & Frame", "Front Body Inspected" },
                    { 1002, "Body & Frame", "Rear Body Inspected" },
                    { 1003, "Body & Frame", "Front Paint Inspected" },
                    { 1004, "Body & Frame", "Rear Paint Inspected" },
                    { 1005, "Body & Frame", "Scratch Repaired" },
                    { 1006, "Body & Frame", "Dent Repaired" },
                    { 1007, "Body & Frame", "Rust Inspected" },
                    { 1008, "Body & Frame", "Rust Repaired" },
                    { 1009, "Body & Frame", "Underbody Inspected" },
                    { 1010, "Body & Frame", "Undercoating Applied" },
                    { 1011, "Body & Frame", "Door Hinges Lubricated" },
                    { 1012, "Body & Frame", "Door Lock Repaired" },
                    { 1013, "Body & Frame", "Window Regulator Replaced" },
                    { 1014, "Body & Frame", "Weather Strip Replaced" },
                    { 1015, "Body & Frame", "Windshield Repaired" },
                    { 1016, "Body & Frame", "Windshield Replaced" },
                    { 1017, "Body & Frame", "Wiper Blades Replaced" },
                    { 1018, "Body & Frame", "Side Mirror Replaced" },
                    { 1019, "Body & Frame", "Hood Latch Serviced" },
                    { 1020, "Body & Frame", "Trunk Latch Serviced" },
                    { 3000, "Brakes", "Brake System Inspected" },
                    { 3001, "Brakes", "Front Brake Pads Replaced" },
                    { 3002, "Brakes", "Rear Brake Pads Replaced" },
                    { 3003, "Brakes", "Front Brake Rotors Replaced" },
                    { 3004, "Brakes", "Rear Brake Rotors Replaced" },
                    { 3005, "Brakes", "Front Brake Rotors Resurfaced" },
                    { 3006, "Brakes", "Rear Brake Rotors Resurfaced" },
                    { 3007, "Brakes", "Brake Fluid Flushed" },
                    { 3008, "Brakes", "Brake Lines Repaired" },
                    { 3009, "Brakes", "Front Brake Hoses Replaced" },
                    { 3010, "Brakes", "Rear Brake Hoses Replaced" },
                    { 3011, "Brakes", "Front Brake Calipers Replaced" },
                    { 3012, "Brakes", "Rear Brake Calipers Replaced" },
                    { 3013, "Brakes", "Parking Brake Adjusted" },
                    { 3014, "Brakes", "Parking Brake Serviced" },
                    { 3015, "Brakes", "ABS System Inspected" },
                    { 3016, "Brakes", "Front ABS Sensor Replaced" },
                    { 3017, "Brakes", "Rear ABS Sensor Replaced" },
                    { 5000, "Electrical", "Battery Inspected" },
                    { 5001, "Electrical", "Battery Replaced" },
                    { 5002, "Electrical", "Battery Terminals Cleaned" },
                    { 5003, "Electrical", "Battery Cables Replaced" },
                    { 5004, "Electrical", "Charging System Tested" },
                    { 5005, "Electrical", "Alternator Replaced" },
                    { 5006, "Electrical", "Starter Replaced" },
                    { 5007, "Electrical", "Fuses Replaced" },
                    { 5008, "Electrical", "Relays Replaced" },
                    { 5009, "Electrical", "Wiring Repaired" },
                    { 5010, "Electrical", "Electrical System Diagnosed" },
                    { 5011, "Electrical", "Headlights Replaced" },
                    { 5012, "Electrical", "Tail Lights Replaced" },
                    { 5013, "Electrical", "Bulbs Replaced" },
                    { 5014, "Electrical", "ECU Diagnosed" },
                    { 5015, "Electrical", "Software Updated" },
                    { 5016, "Electrical", "Key Fob Battery Replaced" },
                    { 7000, "HVAC", "HVAC System Inspected" },
                    { 7001, "HVAC", "Cabin Air Filter Replaced" },
                    { 7002, "HVAC", "AC System Inspected" },
                    { 7003, "HVAC", "AC Refrigerant Recharged" },
                    { 7004, "HVAC", "AC Leak Repaired" },
                    { 7005, "HVAC", "AC Compressor Replaced" },
                    { 7006, "HVAC", "AC Condenser Replaced" },
                    { 7007, "HVAC", "AC Evaporator Replaced" },
                    { 7008, "HVAC", "Heater System Inspected" },
                    { 7009, "HVAC", "Heater Core Replaced" },
                    { 7010, "HVAC", "Blower Motor Replaced" },
                    { 7011, "HVAC", "Climate Control Repaired" },
                    { 9000, "Powertrain", "Oil & Filter Changed" },
                    { 9001, "Powertrain", "Engine Inspected" },
                    { 9002, "Powertrain", "Engine Tuned Up" },
                    { 9003, "Powertrain", "Spark Plugs Replaced" },
                    { 9004, "Powertrain", "Ignition Coils Replaced" },
                    { 9005, "Powertrain", "Engine Air Filter Replaced" },
                    { 9006, "Powertrain", "Fuel Filter Replaced" },
                    { 9007, "Powertrain", "Fuel Injectors Cleaned" },
                    { 9008, "Powertrain", "Fuel Injectors Replaced" },
                    { 9009, "Powertrain", "Throttle Body Cleaned" },
                    { 9010, "Powertrain", "Fuel Pump Replaced" },
                    { 9011, "Powertrain", "Coolant Flushed" },
                    { 9012, "Powertrain", "Coolant Replaced" },
                    { 9013, "Powertrain", "Thermostat Replaced" },
                    { 9014, "Powertrain", "Water Pump Replaced" },
                    { 9015, "Powertrain", "Timing Belt Replaced" },
                    { 9016, "Powertrain", "Timing Chain Serviced" },
                    { 9017, "Powertrain", "Serpentine Belt Replaced" },
                    { 9018, "Powertrain", "Engine Mounts Replaced" },
                    { 9019, "Powertrain", "Engine Leak Repaired" },
                    { 9020, "Powertrain", "Transmission Fluid Serviced" },
                    { 9021, "Powertrain", "Transmission Filter Replaced" },
                    { 9022, "Powertrain", "Transmission Flushed" },
                    { 9023, "Powertrain", "Transmission Repaired" },
                    { 9024, "Powertrain", "Transmission Replaced" },
                    { 9025, "Powertrain", "Clutch Replaced" },
                    { 9026, "Powertrain", "Differential Fluid Serviced" },
                    { 9027, "Powertrain", "Transfer Case Serviced" },
                    { 9028, "Powertrain", "Front CV Axles Replaced" },
                    { 9029, "Powertrain", "Rear CV Axles Replaced" },
                    { 9030, "Powertrain", "Front CV Boots Replaced" },
                    { 9031, "Powertrain", "Rear CV Boots Replaced" },
                    { 11000, "Safety", "Safety Inspection Completed" },
                    { 11001, "Safety", "Airbag System Inspected" },
                    { 11002, "Safety", "Front Airbags Replaced" },
                    { 11003, "Safety", "Side Airbags Replaced" },
                    { 11004, "Safety", "Seat Belts Inspected" },
                    { 11005, "Safety", "Front Seat Belts Replaced" },
                    { 11006, "Safety", "Rear Seat Belts Replaced" },
                    { 11007, "Safety", "ADAS Calibrated" },
                    { 11008, "Safety", "Front Camera Calibrated" },
                    { 11009, "Safety", "Rear Camera Calibrated" },
                    { 11010, "Safety", "Radar Sensor Calibrated" },
                    { 11011, "Safety", "Driver Assistance System Inspected" },
                    { 11012, "Safety", "Recall Inspected" },
                    { 11013, "Safety", "Recall Repair Completed" },
                    { 13000, "Steering", "Steering System Inspected" },
                    { 13001, "Steering", "Power Steering Fluid Serviced" },
                    { 13002, "Steering", "Power Steering Flushed" },
                    { 13003, "Steering", "Steering Rack Replaced" },
                    { 13004, "Steering", "Steering Gear Repaired" },
                    { 13005, "Steering", "Front Tie Rods Replaced" },
                    { 13006, "Steering", "Front Tie Rod Ends Replaced" },
                    { 13007, "Steering", "Steering Linkage Repaired" },
                    { 13008, "Steering", "Steering Column Repaired" },
                    { 13009, "Steering", "Electric Power Steering Diagnosed" },
                    { 15000, "Suspension", "Suspension System Inspected" },
                    { 15001, "Suspension", "Front Shock Absorbers Replaced" },
                    { 15002, "Suspension", "Rear Shock Absorbers Replaced" },
                    { 15003, "Suspension", "Front Struts Replaced" },
                    { 15004, "Suspension", "Rear Struts Replaced" },
                    { 15005, "Suspension", "Front Coil Springs Replaced" },
                    { 15006, "Suspension", "Rear Coil Springs Replaced" },
                    { 15007, "Suspension", "Air Suspension Serviced" },
                    { 15008, "Suspension", "Air Suspension Compressor Replaced" },
                    { 15009, "Suspension", "Front Control Arms Replaced" },
                    { 15010, "Suspension", "Rear Control Arms Replaced" },
                    { 15011, "Suspension", "Front Ball Joints Replaced" },
                    { 15012, "Suspension", "Rear Ball Joints Replaced" },
                    { 15013, "Suspension", "Front Bushings Replaced" },
                    { 15014, "Suspension", "Rear Bushings Replaced" },
                    { 15015, "Suspension", "Front Sway Bar Links Replaced" },
                    { 15016, "Suspension", "Rear Sway Bar Links Replaced" },
                    { 15017, "Suspension", "Front Sway Bar Bushings Replaced" },
                    { 15018, "Suspension", "Rear Sway Bar Bushings Replaced" },
                    { 15019, "Suspension", "Wheel Alignment Completed" },
                    { 15020, "Suspension", "Ride Height Adjusted" },
                    { 17000, "Tires & Wheels", "Tires Inspected" },
                    { 17001, "Tires & Wheels", "Front Tires Inspected" },
                    { 17002, "Tires & Wheels", "Rear Tires Inspected" },
                    { 17003, "Tires & Wheels", "Tires Rotated" },
                    { 17004, "Tires & Wheels", "Front Tires Replaced" },
                    { 17005, "Tires & Wheels", "Rear Tires Replaced" },
                    { 17006, "Tires & Wheels", "Tire Repaired" },
                    { 17007, "Tires & Wheels", "Tires Balanced" },
                    { 17008, "Tires & Wheels", "Tire Pressure Adjusted" },
                    { 17009, "Tires & Wheels", "TPMS Inspected" },
                    { 17010, "Tires & Wheels", "Front TPMS Sensor Replaced" },
                    { 17011, "Tires & Wheels", "Rear TPMS Sensor Replaced" },
                    { 17012, "Tires & Wheels", "Wheels Inspected" },
                    { 17013, "Tires & Wheels", "Front Wheels Replaced" },
                    { 17014, "Tires & Wheels", "Rear Wheels Replaced" },
                    { 17015, "Tires & Wheels", "Wheels Repaired" },
                    { 17016, "Tires & Wheels", "Lug Nuts Replaced" },
                    { 17017, "Tires & Wheels", "Seasonal Tires Changed" },
                    { 99999, "Other", "Other" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_role_claims_role_id",
                schema: "identity",
                table: "role_claims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "identity",
                table: "roles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_service_record_items_service_record_id",
                table: "service_record_items",
                column: "service_record_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_record_items_service_type_id",
                table: "service_record_items",
                column: "service_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_records_vehicle_id",
                table: "service_records",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_claims_user_id",
                schema: "identity",
                table: "user_claims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_logins_user_id",
                schema: "identity",
                table: "user_logins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_role_id",
                schema: "identity",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "identity",
                table: "users",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "identity",
                table: "users",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_user_id",
                table: "vehicles",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "role_claims",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "service_record_items");

            migrationBuilder.DropTable(
                name: "user_claims",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "user_logins",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "user_roles",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "user_tokens",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "service_records");

            migrationBuilder.DropTable(
                name: "service_types");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "vehicles");

            migrationBuilder.DropTable(
                name: "users",
                schema: "identity");
        }
    }
}
