using GarageLog.Core.Validation;

namespace GarageLog.Core.Entities;

public class ServiceRecord
{
    private readonly List<ServiceRecordItem> _items = [];

    // Properties
    public int Id { get; private set; }
    public int VehicleId { get; private set; }
    public DateOnly ServiceDate { get; private set; }
    public int Mileage { get; private set; }
    public bool IsSelfService { get; private set; }
    public string? ShopName { get; private set; }
    public decimal? TotalCost { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation
    public Vehicle Vehicle { get; private set; } = null!;
    public IReadOnlyCollection<ServiceRecordItem> Items => _items.AsReadOnly();

    // Private constructor for EF Core
    private ServiceRecord() { }

    // Internal constructor to ensure only "Vehicle.AddNewServiceRecord" should create these
    internal ServiceRecord(
        int vehicleId,
        DateOnly serviceDate,
        int mileage,
        bool isSelfService,
        decimal? totalCost = null,
        string? shopName = null,
        string? notes = null
    )
    {
        VehicleValidationRules.ValidateMileage(mileage);

        shopName =
            isSelfService ? null
            : string.IsNullOrWhiteSpace(shopName) ? null
            : shopName.Trim();

        notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();

        if (serviceDate > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ArgumentException("Service date cannot be in the future.");

        if (totalCost is < 0)
            throw new ArgumentException("Total cost cannot be negative.");

        if (!isSelfService && shopName is null)
        {
            throw new ArgumentException(
                "Shop name is required for non-self-service records.",
                nameof(shopName)
            );
        }

        VehicleId = vehicleId;
        ServiceDate = serviceDate;
        Mileage = mileage;
        IsSelfService = isSelfService;
        ShopName = shopName;
        TotalCost = totalCost;
        Notes = notes;
        CreatedAt = DateTime.UtcNow;
    }

    // Behavior
    public ServiceRecordItem AddServiceItem(ServiceType serviceType, string? customName = null)
    {
        var item = new ServiceRecordItem(serviceType, customName);

        _items.Add(item);

        return item;
    }
}
