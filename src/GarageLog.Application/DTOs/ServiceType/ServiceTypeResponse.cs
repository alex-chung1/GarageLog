namespace GarageLog.Application.DTOs.ServiceType;

public class ServiceTypeResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Category { get; set; }
    public bool IsCustomEntry { get; set; }
}
