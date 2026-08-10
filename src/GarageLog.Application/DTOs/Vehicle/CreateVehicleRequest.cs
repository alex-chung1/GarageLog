using System.ComponentModel.DataAnnotations;
using GarageLog.Core.Enums;

namespace GarageLog.Application.DTOs.Vehicle;

public class CreateVehicleRequest
{
    [Required]
    [EnumDataType(typeof(VehicleType))]
    public VehicleType Type { get; set; }

    [Required]
    [MaxLength(50)]
    public string Make { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Model { get; set; } = string.Empty;

    [Range(1886, 2100)]
    public int Year { get; set; }

    [RegularExpression(@"^[A-HJ-NPR-Z0-9]{17}$", ErrorMessage = "VIN must be a valid 17-character VIN.")]
    public string? Vin { get; set; }
}
