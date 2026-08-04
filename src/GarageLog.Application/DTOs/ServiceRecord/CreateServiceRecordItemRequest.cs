namespace GarageLog.Application.DTOs.ServiceRecord;

public class CreateServiceRecordItemRequest
{
    public int ServiceTypeId { get; set; }
    public string? CustomName { get; set; }
}
