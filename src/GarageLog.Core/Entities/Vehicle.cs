using GarageLog.Core.Enums;
using GarageLog.Core.Validation;

namespace GarageLog.Core.Entities;

public class Vehicle
{
    private readonly List<ServiceRecord> _serviceRecords = [];

    // Properties
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public VehicleType Type { get; private set; }
    public string Make { get; private set; } = string.Empty;
    public string Model { get; private set; } = string.Empty;
    public int Year { get; private set; }
    public string? Vin { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation
    public IReadOnlyCollection<ServiceRecord> ServiceRecords => _serviceRecords.AsReadOnly();

    // Private constructor for EF Core
    private Vehicle() { }

    // Private constructor used by factory method
    private Vehicle(
        int userId,
        VehicleType type,
        string make,
        string model,
        int year,
        string? vin = null
    )
    {
        VehicleValidationRules.ValidateMakeModel(make, model);
        VehicleValidationRules.ValidateYear(year);

        UserId = userId;
        Type = type;
        Make = make;
        Model = model;
        Year = year;
        Vin = string.IsNullOrWhiteSpace(vin) ? null : vin;
        CreatedAt = DateTime.UtcNow;
    }

    // Factory method to create a new vehicle for user
    public static Vehicle Create(
        int userId,
        VehicleType type,
        string make,
        string model,
        int year,
        string? vin = null
    )
    {
        return new Vehicle(userId, type, make, model, year, vin);
    }

    // Adds a new service record to this vehicle
    public ServiceRecord AddServiceRecord(
        DateOnly serviceDate,
        int mileage,
        bool isSelfService,
        decimal? totalCost = null,
        string? shopName = null,
        string? notes = null
    )
    {
        var serviceRecord = new ServiceRecord(
            this.Id,
            serviceDate,
            mileage,
            isSelfService,
            totalCost,
            shopName,
            notes
        );

        _serviceRecords.Add(serviceRecord);

        return serviceRecord;
    }

    public void UpdateDetails(VehicleType type, string make, string model, int year, string? vin)
    {
        VehicleValidationRules.ValidateMakeModel(make, model);
        VehicleValidationRules.ValidateYear(year);

        Type = type;
        Make = make;
        Model = model;
        Year = year;
        Vin = string.IsNullOrWhiteSpace(vin) ? null : vin;
    }

    public void ValidateHistoricalMileage(int mileage, ServiceRecord? previous, ServiceRecord? next)
    {
        VehicleValidationRules.ValidateMileage(mileage);

        if (previous is not null && mileage < previous.Mileage)
        {
            throw new InvalidOperationException(
                "Service mileage cannot be lower than the previous service record mileage."
            );
        }

        if (next is not null && mileage > next.Mileage)
        {
            throw new InvalidOperationException(
                "Service mileage cannot be greater than the next service record mileage."
            );
        }
    }
}
