using GarageLog.Core.Enums;

namespace GarageLog.Core.Entities;

public class Vehicle
{
    private const int MaxReasonableMileage = 5_000_000;
    private readonly List<ServiceRecord> _serviceRecords = [];

    // Properties
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public VehicleType Type { get; private set; }
    public string Make { get; private set; } = string.Empty;
    public string Model { get; private set; } = string.Empty;
    public int Year { get; private set; }
    public string? Vin { get; private set; }
    public int? CurrentMileage { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation
    public IReadOnlyCollection<ServiceRecord> ServiceRecords => _serviceRecords.AsReadOnly();

    // Default Constructor for EF Core
    private Vehicle() { }

    // Constructor
    public Vehicle(
        int userId,
        VehicleType type,
        string make,
        string model,
        int year,
        string? vin = null,
        int? currentMileage = null
    )
    {
        ValidateYear(year);
        ValidateMileage(currentMileage);

        UserId = userId;
        Type = type;
        Make = make;
        Model = model;
        Year = year;
        Vin = string.IsNullOrWhiteSpace(vin) ? null : vin;
        CurrentMileage = currentMileage;
        CreatedAt = DateTime.UtcNow;
    }

    // Domain behavior
    public void UpdateDetails(VehicleType type, string make, string model, int year, string? vin)
    {
        ValidateYear(year);

        Type = type;
        Make = make;
        Model = model;
        Year = year;
        Vin = string.IsNullOrWhiteSpace(vin) ? null : vin;
    }

    public ServiceRecord AddServiceRecord(
        DateTime serviceDate,
        int mileage,
        bool isSelfService,
        decimal? totalCost = null,
        string? shopName = null,
        string? notes = null
    )
    {
        var serviceRecord = new ServiceRecord(
            Id,
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

    public void UpdateMileage(int newMileage)
    {
        ValidateMileage(newMileage);

        if (newMileage < CurrentMileage)
        {
            throw new InvalidOperationException("Mileage cannot decrease.");
        }

        CurrentMileage = newMileage;
    }

    private static void ValidateYear(int year)
    {
        if (year < 1886 || year > DateTime.UtcNow.Year + 1)
        {
            throw new ArgumentException("Year is not a valid vehicle year.", nameof(year));
        }
    }

    private static void ValidateMileage(int? mileage)
    {
        if (mileage is < 0 or > MaxReasonableMileage)
        {
            throw new ArgumentException(
                $"Mileage must be between 0 and {MaxReasonableMileage:N0}.",
                nameof(mileage)
            );
        }
    }

    public void ValidateHistoricalMileage(int mileage, ServiceRecord? previous, ServiceRecord? next)
    {
        ValidateMileage(mileage);

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
