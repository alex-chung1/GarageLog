namespace GarageLog.Application.DTOs.ServiceRecord;

public class CreateServiceRecordItemRequest
{
    public int ServiceTypeId { get; set; }

    public int Quantity { get; set; } = 1;

    public string? CustomName { get; set; }
}
