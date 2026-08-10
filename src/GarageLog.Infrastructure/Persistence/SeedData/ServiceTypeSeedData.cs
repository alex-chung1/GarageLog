using GarageLog.Core.Entities;

using Microsoft.EntityFrameworkCore;

namespace GarageLog.Infrastructure.Persistence.SeedData;

public static class ServiceTypeSeedData
{
    public static void Seed(ModelBuilder builder)
    {
        // Service types are application reference data.
        // IDs are grouped by category and should remain stable because
        // future features may reference these values.

        builder
            .Entity<ServiceType>()
            .HasData(
                // COMMON / POPULAR SERVICES (1-999)
                new { Id = 1, Name = "Oil Change", Category = "Engine" },
                new { Id = 2, Name = "Tire Rotation", Category = "Tires" },
                new { Id = 3, Name = "Brake Pad Replacement", Category = "Brakes" },

                // BODY & FRAME (1000-2999)
                new { Id = 1000, Name = "Body Inspected", Category = "Body & Frame" },
                new { Id = 1001, Name = "Front Body Inspected", Category = "Body & Frame" },
                new { Id = 1002, Name = "Rear Body Inspected", Category = "Body & Frame" },
                new { Id = 1003, Name = "Front Paint Inspected", Category = "Body & Frame" },
                new { Id = 1004, Name = "Rear Paint Inspected", Category = "Body & Frame" },
                new { Id = 1005, Name = "Scratch Repaired", Category = "Body & Frame" },
                new { Id = 1006, Name = "Dent Repaired", Category = "Body & Frame" },
                new { Id = 1007, Name = "Rust Inspected", Category = "Body & Frame" },
                new { Id = 1008, Name = "Rust Repaired", Category = "Body & Frame" },
                new { Id = 1009, Name = "Underbody Inspected", Category = "Body & Frame" },
                new { Id = 1010, Name = "Undercoating Applied", Category = "Body & Frame" },
                new { Id = 1011, Name = "Door Hinges Lubricated", Category = "Body & Frame" },
                new { Id = 1012, Name = "Door Lock Repaired", Category = "Body & Frame" },
                new { Id = 1013, Name = "Window Regulator Replaced", Category = "Body & Frame" },
                new { Id = 1014, Name = "Weather Strip Replaced", Category = "Body & Frame" },
                new { Id = 1015, Name = "Windshield Repaired", Category = "Body & Frame" },
                new { Id = 1016, Name = "Windshield Replaced", Category = "Body & Frame" },
                new { Id = 1017, Name = "Wiper Blades Replaced", Category = "Body & Frame" },
                new { Id = 1018, Name = "Side Mirror Replaced", Category = "Body & Frame" },
                new { Id = 1019, Name = "Hood Latch Serviced", Category = "Body & Frame" },
                new { Id = 1020, Name = "Trunk Latch Serviced", Category = "Body & Frame" },

                // BRAKES (3000-4999)
                new { Id = 3000, Name = "Brake System Inspected", Category = "Brakes" },
                new { Id = 3001, Name = "Front Brake Pads Replaced", Category = "Brakes" },
                new { Id = 3002, Name = "Rear Brake Pads Replaced", Category = "Brakes" },
                new { Id = 3003, Name = "Front Brake Rotors Replaced", Category = "Brakes" },
                new { Id = 3004, Name = "Rear Brake Rotors Replaced", Category = "Brakes" },
                new { Id = 3005, Name = "Front Brake Rotors Resurfaced", Category = "Brakes" },
                new { Id = 3006, Name = "Rear Brake Rotors Resurfaced", Category = "Brakes" },
                new { Id = 3007, Name = "Brake Fluid Flushed", Category = "Brakes" },
                new { Id = 3008, Name = "Brake Lines Repaired", Category = "Brakes" },
                new { Id = 3009, Name = "Front Brake Hoses Replaced", Category = "Brakes" },
                new { Id = 3010, Name = "Rear Brake Hoses Replaced", Category = "Brakes" },
                new { Id = 3011, Name = "Front Brake Calipers Replaced", Category = "Brakes" },
                new { Id = 3012, Name = "Rear Brake Calipers Replaced", Category = "Brakes" },
                new { Id = 3013, Name = "Parking Brake Adjusted", Category = "Brakes" },
                new { Id = 3014, Name = "Parking Brake Serviced", Category = "Brakes" },
                new { Id = 3015, Name = "ABS System Inspected", Category = "Brakes" },
                new { Id = 3016, Name = "Front ABS Sensor Replaced", Category = "Brakes" },
                new { Id = 3017, Name = "Rear ABS Sensor Replaced", Category = "Brakes" },

                // ELECTRICAL (5000-6999)
                new { Id = 5000, Name = "Battery Inspected", Category = "Electrical" },
                new { Id = 5001, Name = "Battery Replaced", Category = "Electrical" },
                new { Id = 5002, Name = "Battery Terminals Cleaned", Category = "Electrical" },
                new { Id = 5003, Name = "Battery Cables Replaced", Category = "Electrical" },
                new { Id = 5004, Name = "Charging System Tested", Category = "Electrical" },
                new { Id = 5005, Name = "Alternator Replaced", Category = "Electrical" },
                new { Id = 5006, Name = "Starter Replaced", Category = "Electrical" },
                new { Id = 5007, Name = "Fuses Replaced", Category = "Electrical" },
                new { Id = 5008, Name = "Relays Replaced", Category = "Electrical" },
                new { Id = 5009, Name = "Wiring Repaired", Category = "Electrical" },
                new { Id = 5010, Name = "Electrical System Diagnosed", Category = "Electrical" },
                new { Id = 5011, Name = "Headlights Replaced", Category = "Electrical" },
                new { Id = 5012, Name = "Tail Lights Replaced", Category = "Electrical" },
                new { Id = 5013, Name = "Bulbs Replaced", Category = "Electrical" },
                new { Id = 5014, Name = "ECU Diagnosed", Category = "Electrical" },
                new { Id = 5015, Name = "Software Updated", Category = "Electrical" },
                new { Id = 5016, Name = "Key Fob Battery Replaced", Category = "Electrical" },

                // HVAC (7000-8999)
                new { Id = 7000, Name = "HVAC System Inspected", Category = "HVAC" },
                new { Id = 7001, Name = "Cabin Air Filter Replaced", Category = "HVAC" },
                new { Id = 7002, Name = "AC System Inspected", Category = "HVAC" },
                new { Id = 7003, Name = "AC Refrigerant Recharged", Category = "HVAC" },
                new { Id = 7004, Name = "AC Leak Repaired", Category = "HVAC" },
                new { Id = 7005, Name = "AC Compressor Replaced", Category = "HVAC" },
                new { Id = 7006, Name = "AC Condenser Replaced", Category = "HVAC" },
                new { Id = 7007, Name = "AC Evaporator Replaced", Category = "HVAC" },
                new { Id = 7008, Name = "Heater System Inspected", Category = "HVAC" },
                new { Id = 7009, Name = "Heater Core Replaced", Category = "HVAC" },
                new { Id = 7010, Name = "Blower Motor Replaced", Category = "HVAC" },
                new { Id = 7011, Name = "Climate Control Repaired", Category = "HVAC" },

                // POWERTRAIN (9000-10999)
                new { Id = 9000, Name = "Oil & Filter Changed", Category = "Powertrain" },
                new { Id = 9001, Name = "Engine Inspected", Category = "Powertrain" },
                new { Id = 9002, Name = "Engine Tuned Up", Category = "Powertrain" },
                new { Id = 9003, Name = "Spark Plugs Replaced", Category = "Powertrain" },
                new { Id = 9004, Name = "Ignition Coils Replaced", Category = "Powertrain" },
                new { Id = 9005, Name = "Engine Air Filter Replaced", Category = "Powertrain" },
                new { Id = 9006, Name = "Fuel Filter Replaced", Category = "Powertrain" },
                new { Id = 9007, Name = "Fuel Injectors Cleaned", Category = "Powertrain" },
                new { Id = 9008, Name = "Fuel Injectors Replaced", Category = "Powertrain" },
                new { Id = 9009, Name = "Throttle Body Cleaned", Category = "Powertrain" },
                new { Id = 9010, Name = "Fuel Pump Replaced", Category = "Powertrain" },
                new { Id = 9011, Name = "Coolant Flushed", Category = "Powertrain" },
                new { Id = 9012, Name = "Coolant Replaced", Category = "Powertrain" },
                new { Id = 9013, Name = "Thermostat Replaced", Category = "Powertrain" },
                new { Id = 9014, Name = "Water Pump Replaced", Category = "Powertrain" },
                new { Id = 9015, Name = "Timing Belt Replaced", Category = "Powertrain" },
                new { Id = 9016, Name = "Timing Chain Serviced", Category = "Powertrain" },
                new { Id = 9017, Name = "Serpentine Belt Replaced", Category = "Powertrain" },
                new { Id = 9018, Name = "Engine Mounts Replaced", Category = "Powertrain" },
                new { Id = 9019, Name = "Engine Leak Repaired", Category = "Powertrain" },
                new { Id = 9020, Name = "Transmission Fluid Serviced", Category = "Powertrain" },
                new { Id = 9021, Name = "Transmission Filter Replaced", Category = "Powertrain" },
                new { Id = 9022, Name = "Transmission Flushed", Category = "Powertrain" },
                new { Id = 9023, Name = "Transmission Repaired", Category = "Powertrain" },
                new { Id = 9024, Name = "Transmission Replaced", Category = "Powertrain" },
                new { Id = 9025, Name = "Clutch Replaced", Category = "Powertrain" },
                new { Id = 9026, Name = "Differential Fluid Serviced", Category = "Powertrain" },
                new { Id = 9027, Name = "Transfer Case Serviced", Category = "Powertrain" },
                new { Id = 9028, Name = "Front CV Axles Replaced", Category = "Powertrain" },
                new { Id = 9029, Name = "Rear CV Axles Replaced", Category = "Powertrain" },
                new { Id = 9030, Name = "Front CV Boots Replaced", Category = "Powertrain" },
                new { Id = 9031, Name = "Rear CV Boots Replaced", Category = "Powertrain" },

                // SAFETY (11000-12999)
                new { Id = 11000, Name = "Safety Inspection Completed", Category = "Safety" },
                new { Id = 11001, Name = "Airbag System Inspected", Category = "Safety" },
                new { Id = 11002, Name = "Front Airbags Replaced", Category = "Safety" },
                new { Id = 11003, Name = "Side Airbags Replaced", Category = "Safety" },
                new { Id = 11004, Name = "Seat Belts Inspected", Category = "Safety" },
                new { Id = 11005, Name = "Front Seat Belts Replaced", Category = "Safety" },
                new { Id = 11006, Name = "Rear Seat Belts Replaced", Category = "Safety" },
                new { Id = 11007, Name = "ADAS Calibrated", Category = "Safety" },
                new { Id = 11008, Name = "Front Camera Calibrated", Category = "Safety" },
                new { Id = 11009, Name = "Rear Camera Calibrated", Category = "Safety" },
                new { Id = 11010, Name = "Radar Sensor Calibrated", Category = "Safety" },
                new { Id = 11011, Name = "Driver Assistance System Inspected", Category = "Safety" },
                new { Id = 11012, Name = "Recall Inspected", Category = "Safety" },
                new { Id = 11013, Name = "Recall Repair Completed", Category = "Safety" },

                // STEERING (13000-14999)
                new { Id = 13000, Name = "Steering System Inspected", Category = "Steering" },
                new { Id = 13001, Name = "Power Steering Fluid Serviced", Category = "Steering" },
                new { Id = 13002, Name = "Power Steering Flushed", Category = "Steering" },
                new { Id = 13003, Name = "Steering Rack Replaced", Category = "Steering" },
                new { Id = 13004, Name = "Steering Gear Repaired", Category = "Steering" },
                new { Id = 13005, Name = "Front Tie Rods Replaced", Category = "Steering" },
                new { Id = 13006, Name = "Front Tie Rod Ends Replaced", Category = "Steering" },
                new { Id = 13007, Name = "Steering Linkage Repaired", Category = "Steering" },
                new { Id = 13008, Name = "Steering Column Repaired", Category = "Steering" },
                new { Id = 13009, Name = "Electric Power Steering Diagnosed", Category = "Steering" },

                // SUSPENSION (15000-16999)
                new { Id = 15000, Name = "Suspension System Inspected", Category = "Suspension" },
                new { Id = 15001, Name = "Front Shock Absorbers Replaced", Category = "Suspension" },
                new { Id = 15002, Name = "Rear Shock Absorbers Replaced", Category = "Suspension" },
                new { Id = 15003, Name = "Front Struts Replaced", Category = "Suspension" },
                new { Id = 15004, Name = "Rear Struts Replaced", Category = "Suspension" },
                new { Id = 15005, Name = "Front Coil Springs Replaced", Category = "Suspension" },
                new { Id = 15006, Name = "Rear Coil Springs Replaced", Category = "Suspension" },
                new { Id = 15007, Name = "Air Suspension Serviced", Category = "Suspension" },
                new { Id = 15008, Name = "Air Suspension Compressor Replaced", Category = "Suspension" },
                new { Id = 15009, Name = "Front Control Arms Replaced", Category = "Suspension" },
                new { Id = 15010, Name = "Rear Control Arms Replaced", Category = "Suspension" },
                new { Id = 15011, Name = "Front Ball Joints Replaced", Category = "Suspension" },
                new { Id = 15012, Name = "Rear Ball Joints Replaced", Category = "Suspension" },
                new { Id = 15013, Name = "Front Bushings Replaced", Category = "Suspension" },
                new { Id = 15014, Name = "Rear Bushings Replaced", Category = "Suspension" },
                new { Id = 15015, Name = "Front Sway Bar Links Replaced", Category = "Suspension" },
                new { Id = 15016, Name = "Rear Sway Bar Links Replaced", Category = "Suspension" },
                new { Id = 15017, Name = "Front Sway Bar Bushings Replaced", Category = "Suspension" },
                new { Id = 15018, Name = "Rear Sway Bar Bushings Replaced", Category = "Suspension" },
                new { Id = 15019, Name = "Wheel Alignment Completed", Category = "Suspension" },
                new { Id = 15020, Name = "Ride Height Adjusted", Category = "Suspension" },

                // TIRES & WHEELS (17000-18999)
                new { Id = 17000, Name = "Tires Inspected", Category = "Tires & Wheels" },
                new { Id = 17001, Name = "Front Tires Inspected", Category = "Tires & Wheels" },
                new { Id = 17002, Name = "Rear Tires Inspected", Category = "Tires & Wheels" },
                new { Id = 17003, Name = "Tires Rotated", Category = "Tires & Wheels" },
                new { Id = 17004, Name = "Front Tires Replaced", Category = "Tires & Wheels" },
                new { Id = 17005, Name = "Rear Tires Replaced", Category = "Tires & Wheels" },
                new { Id = 17006, Name = "Tire Repaired", Category = "Tires & Wheels" },
                new { Id = 17007, Name = "Tires Balanced", Category = "Tires & Wheels" },
                new { Id = 17008, Name = "Tire Pressure Adjusted", Category = "Tires & Wheels" },
                new { Id = 17009, Name = "TPMS Inspected", Category = "Tires & Wheels" },
                new { Id = 17010, Name = "Front TPMS Sensor Replaced", Category = "Tires & Wheels" },
                new { Id = 17011, Name = "Rear TPMS Sensor Replaced", Category = "Tires & Wheels" },
                new { Id = 17012, Name = "Wheels Inspected", Category = "Tires & Wheels" },
                new { Id = 17013, Name = "Front Wheels Replaced", Category = "Tires & Wheels" },
                new { Id = 17014, Name = "Rear Wheels Replaced", Category = "Tires & Wheels" },
                new { Id = 17015, Name = "Wheels Repaired", Category = "Tires & Wheels" },
                new { Id = 17016, Name = "Lug Nuts Replaced", Category = "Tires & Wheels" },
                new { Id = 17017, Name = "Seasonal Tires Changed", Category = "Tires & Wheels" },

                // FALLBACK
                new { Id = 99999, Name = "Other", Category = "Other" }
            );
    }
}
