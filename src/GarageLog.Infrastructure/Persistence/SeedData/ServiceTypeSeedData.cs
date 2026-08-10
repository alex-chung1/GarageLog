using GarageLog.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GarageLog.Infrastructure.Persistence.SeedData;

public static class ServiceTypeSeedData
{
    public static void Seed(ModelBuilder builder)
    {
        // Service types are application reference data.
        // IDs should remain stable because future features may
        // reference these values, such as maintenance reminders,
        // service history, and reporting.

        builder
            .Entity<ServiceType>()
            .HasData(
                new
                {
                    Id = 1,
                    Name = "Oil Change",
                    Category = "Engine",
                },
                new
                {
                    Id = 2,
                    Name = "Tire Rotation",
                    Category = "Tires",
                },
                new
                {
                    Id = 3,
                    Name = "Brake Pad Replacement",
                    Category = "Brakes",
                },
                new
                {
                    Id = 4,
                    Name = "Brake Fluid Flush",
                    Category = "Brakes",
                },
                new
                {
                    Id = 5,
                    Name = "Battery Replacement",
                    Category = "Electrical",
                },
                new
                {
                    Id = 6,
                    Name = "Air Filter Replacement",
                    Category = "Engine",
                },
                new
                {
                    Id = 7,
                    Name = "Cabin Air Filter Replacement",
                    Category = "Interior",
                },
                new
                {
                    Id = 8,
                    Name = "Coolant Flush",
                    Category = "Cooling System",
                },
                new
                {
                    Id = 9,
                    Name = "Transmission Service",
                    Category = "Transmission",
                },
                new
                {
                    Id = 10,
                    Name = "Spark Plug Replacement",
                    Category = "Engine",
                },
                new
                {
                    Id = 9999,
                    Name = "Other",
                    Category = "Other",
                }
            );
    }
}
