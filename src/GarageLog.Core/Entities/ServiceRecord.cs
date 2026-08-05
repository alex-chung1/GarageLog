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
        ServiceRecordValidationRules.ValidateServiceDate(serviceDate);
        ServiceRecordValidationRules.ValidateTotalCost(totalCost);

        shopName = ServiceRecordValidationRules.NormalizeShopName(isSelfService, shopName);
        notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();

        VehicleId = vehicleId;
        ServiceDate = serviceDate;
        Mileage = mileage;
        IsSelfService = isSelfService;
        ShopName = shopName;
        TotalCost = totalCost;
        Notes = notes;
        CreatedAt = DateTime.UtcNow;
    }

    // Adds a new service items to this service record
    public ServiceRecordItem AddServiceItem(ServiceType serviceType, string? customName = null)
    {
        var item = new ServiceRecordItem(serviceType, customName);

        _items.Add(item);

        return item;
    }

    public void UpdateDetails(
        DateOnly serviceDate,
        int mileage,
        bool isSelfService,
        decimal? totalCost,
        string? shopName,
        string? notes
    )
    {
        VehicleValidationRules.ValidateMileage(mileage);
        ServiceRecordValidationRules.ValidateServiceDate(serviceDate);
        ServiceRecordValidationRules.ValidateTotalCost(totalCost);

        ServiceDate = serviceDate;
        Mileage = mileage;
        IsSelfService = isSelfService;
        ShopName = ServiceRecordValidationRules.NormalizeShopName(isSelfService, shopName);
        TotalCost = totalCost;
        Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
    }

    public void ReplaceItems(IEnumerable<(ServiceType ServiceType, string? CustomName)> items)
    {
        _items.Clear();

        foreach (var (serviceType, customName) in items)
        {
            _items.Add(new ServiceRecordItem(serviceType, customName));
        }
    }
}
