namespace GarageLog.Core.Entities;

public class ServiceRecordItem
{
    // Properties
    public int Id { get; private set; }
    public int ServiceRecordId { get; private set; }
    public int ServiceTypeId { get; private set; }
    public string? CustomName { get; private set; }
    public int Quantity { get; private set; }

    // Navigation
    public ServiceRecord ServiceRecord { get; private set; } = null!;
    public ServiceType ServiceType { get; private set; } = null!;

    // Default Constructor for EF Core
    private ServiceRecordItem() { }

    // Constructor — internal: only ServiceRecord.AddServiceItem should create these
    internal ServiceRecordItem(ServiceType serviceType, int quantity = 1, string? customName = null)
    {
        ArgumentNullException.ThrowIfNull(serviceType);

        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be positive.", nameof(quantity));
        }

        string? trimmedCustomName = string.IsNullOrWhiteSpace(customName)
            ? null
            : customName.Trim();

        switch (serviceType.IsCustomEntry)
        {
            case true when trimmedCustomName is null:
                throw new ArgumentException(
                    "CustomName is required when ServiceType is Custom.",
                    nameof(customName)
                );
            case false when trimmedCustomName is not null:
                throw new ArgumentException(
                    "CustomName can only be set when ServiceType is Custom.",
                    nameof(customName)
                );
        }

        ServiceTypeId = serviceType.Id;
        Quantity = quantity;
        CustomName = trimmedCustomName;
    }
}
