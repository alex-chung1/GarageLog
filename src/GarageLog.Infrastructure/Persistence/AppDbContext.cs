using GarageLog.Core.Entities;
using GarageLog.Infrastructure.Identity;
using GarageLog.Infrastructure.Persistence.SeedData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GarageLog.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions options)
        : base(options) { }

    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<ServiceRecord> ServiceRecords { get; set; }
    public DbSet<ServiceRecordItem> ServiceRecordItems { get; set; }
    public DbSet<ServiceType> ServiceTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Identity tables
        builder.Entity<ApplicationUser>().ToTable("users", "identity");
        builder.Entity<IdentityRole<int>>().ToTable("roles", "identity");
        builder.Entity<IdentityUserRole<int>>().ToTable("user_roles", "identity");
        builder.Entity<IdentityUserClaim<int>>().ToTable("user_claims", "identity");
        builder.Entity<IdentityUserLogin<int>>().ToTable("user_logins", "identity");
        builder.Entity<IdentityUserToken<int>>().ToTable("user_tokens", "identity");
        builder.Entity<IdentityRoleClaim<int>>().ToTable("role_claims", "identity");

        // Vehicle -> User
        builder
            .Entity<Vehicle>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Vehicle -> ServiceRecords
        builder
            .Entity<Vehicle>()
            .HasMany(v => v.ServiceRecords)
            .WithOne(sr => sr.Vehicle)
            .HasForeignKey(sr => sr.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        // ServiceRecord -> Items
        builder
            .Entity<ServiceRecord>()
            .HasMany(sr => sr.Items)
            .WithOne(i => i.ServiceRecord)
            .HasForeignKey(i => i.ServiceRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        // ServiceType -> Items
        builder
            .Entity<ServiceType>()
            .HasMany(st => st.ServiceRecordItems)
            .WithOne(i => i.ServiceType)
            .HasForeignKey(i => i.ServiceTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed data
        ServiceTypeSeedData.Seed(builder);
    }
}
