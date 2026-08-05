using System.ComponentModel.DataAnnotations;

namespace GarageLog.Application.DTOs.ServiceRecord;

public class CreateServiceRecordItemRequest
{
    [Required]
    public int ServiceTypeId { get; set; }

    [MaxLength(100)]
    public string? CustomName { get; set; }
}
