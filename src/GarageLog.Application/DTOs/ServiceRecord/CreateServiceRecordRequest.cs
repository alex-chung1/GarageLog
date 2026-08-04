using GarageLog.Core.Enums;

namespace GarageLog.Application.DTOs.ServiceRecord;

public class CreateServiceRecordRequest
{
    public DateTime ServiceDate { get; set; }

    public int Mileage { get; set; }

    public bool IsSelfService { get; set; }

    public decimal? TotalCost { get; set; }

    public string? ShopName { get; set; }

    public string? Notes { get; set; }

    public List<CreateServiceRecordItemRequest> Items { get; set; } = [];
}
