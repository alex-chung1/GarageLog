using System.ComponentModel.DataAnnotations;

namespace GarageLog.Application.DTOs.ServiceRecord;

public class UpdateServiceRecordRequest
{
    [Required]
    public DateOnly ServiceDate { get; set; }

    [Range(0, int.MaxValue)]
    public int Mileage { get; set; }

    public bool IsSelfService { get; set; }

    [MaxLength(100)]
    public string? ShopName { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? TotalCost { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one service item is required.")]
    public List<CreateServiceRecordItemRequest> Items { get; set; } = [];
}
