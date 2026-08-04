namespace GarageLog.Application.DTOs.ServiceRecord;

public class ServiceRecordResponse
{
    public int Id { get; set; }

    public DateTime ServiceDate { get; set; }

    public int Mileage { get; set; }

    public decimal? TotalCost { get; set; }

    public bool IsSelfService { get; set; }

    public string? ShopName { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public IEnumerable<ServiceRecordItemResponse> Items { get; set; } = [];
}
