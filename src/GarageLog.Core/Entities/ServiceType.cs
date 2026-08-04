namespace GarageLog.Core.Entities;

public class ServiceType
{
    // Properties
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public bool IsCustomEntry { get; private set; }

    // Navigation
    public ICollection<ServiceRecordItem> ServiceRecordItems { get; private set; } = [];

    // EF Core constructor
    private ServiceType() { }

    // Domain constructor
    public ServiceType(string name, string category, bool isCustomEntry = false)
    {
        Name = name;
        Category = category;
        IsCustomEntry = isCustomEntry;
    }
}
