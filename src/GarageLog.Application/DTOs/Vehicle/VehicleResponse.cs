using GarageLog.Core.Enums;

namespace GarageLog.Application.DTOs.Vehicle;

public class VehicleResponse
{
    public int Id { get; set; }

    public VehicleType Type { get; set; }

    public string Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int Year { get; set; }

    public string? Vin { get; set; }

    public int? Mileage { get; set; }

    public DateTime CreatedAt { get; set; }
}
