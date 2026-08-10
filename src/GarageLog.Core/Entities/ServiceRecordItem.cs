namespace GarageLog.Core.Entities;

public class ServiceRecordItem
{
    // Temporary until these become seeded constants/enums
    private const int CustomServiceTypeId = 99999;

    // Properties
    public int Id { get; private set; }
    public int ServiceRecordId { get; private set; }
    public int ServiceTypeId { get; private set; }
    public string? CustomName { get; private set; }

    // Navigation
    public ServiceRecord ServiceRecord { get; private set; } = null!;
    public ServiceType ServiceType { get; private set; } = null!;

    // EF Core constructor
    private ServiceRecordItem() { }

    // Only ServiceRecord.AddServiceItem should create these
    internal ServiceRecordItem(ServiceType serviceType, string? customName = null)
    {
        ArgumentNullException.ThrowIfNull(serviceType);

        customName = string.IsNullOrWhiteSpace(customName) ? null : customName.Trim();

        if (serviceType.Id == CustomServiceTypeId && customName is null)
        {
            throw new ArgumentException(
                "A custom service name is required when the service type is custom."
            );
        }

        if (serviceType.Id != CustomServiceTypeId)
        {
            customName = null;
        }

        ServiceTypeId = serviceType.Id;
        CustomName = customName;
    }
}
