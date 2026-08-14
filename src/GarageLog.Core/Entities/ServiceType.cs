namespace GarageLog.Core.Entities;

public class ServiceType
{
    // Properties
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;

    // Navigation
    public ICollection<ServiceRecordItem> ServiceRecordItems { get; private set; } = [];

    // Private constructor for EF Core
    private ServiceType()
    {
    }

    // Domain constructor
    public ServiceType(string name, string category)
    {
        Name = name;
        Category = category;
    }
}
