namespace GarageLog.Core.Entities;

public class ServiceRecord
{
    private const int MaxReasonableMileage = 5_000_000;
    private readonly List<ServiceRecordItem> _items = [];

    // Properties
    public int Id { get; private set; }
    public int VehicleId { get; private set; }
    public DateTime ServiceDate { get; private set; }
    public int Mileage { get; private set; }
    public decimal? TotalCost { get; private set; }
    public bool IsSelfService { get; private set; }
    public string? ShopName { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation
    public Vehicle Vehicle { get; private set; } = null!;
    public IReadOnlyCollection<ServiceRecordItem> Items => _items.AsReadOnly();

    // Default Constructor for EF Core
    private ServiceRecord() { }

    // Constructor — internal: only Vehicle.AddServiceRecord should create these
    internal ServiceRecord(
        int vehicleId,
        DateTime serviceDate,
        int mileage,
        bool isSelfService,
        decimal? totalCost = null,
        string? shopName = null,
        string? notes = null
    )
    {
        if (mileage is < 0 or > MaxReasonableMileage)
        {
            throw new ArgumentException(
                $"Mileage must be between 0 and {MaxReasonableMileage:N0}.",
                nameof(mileage)
            );
        }

        if (serviceDate > DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Service date cannot be in the future.",
                nameof(serviceDate)
            );
        }

        if (totalCost is < 0)
        {
            throw new ArgumentException("Total cost cannot be negative.", nameof(totalCost));
        }

        switch (isSelfService)
        {
            case false when string.IsNullOrWhiteSpace(shopName):
                throw new ArgumentException(
                    "Shop name is required for non-self-service records.",
                    nameof(shopName)
                );
            case true when !string.IsNullOrWhiteSpace(shopName):
                throw new ArgumentException(
                    "Shop name cannot be set for self-service records.",
                    nameof(shopName)
                );
        }

        VehicleId = vehicleId;
        ServiceDate = serviceDate;
        Mileage = mileage;
        IsSelfService = isSelfService;
        TotalCost = totalCost;
        ShopName = isSelfService ? null : shopName!.Trim();
        Notes = notes;
        CreatedAt = DateTime.UtcNow;
    }

    // Behavior
    public ServiceRecordItem AddServiceItem(
        ServiceType serviceType,
        int quantity = 1,
        string? customName = null
    )
    {
        var item = new ServiceRecordItem(serviceType, quantity, customName);

        _items.Add(item);

        return item;
    }
}
