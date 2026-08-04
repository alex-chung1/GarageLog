namespace GarageLog.Application.DTOs.ServiceRecord;

public class ServiceRecordItemResponse
{
    public int Id { get; set; }

    public int ServiceTypeId { get; set; }

    public string ServiceTypeName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public string? CustomName { get; set; }
}
