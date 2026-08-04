using GarageLog.Application.DTOs.Vehicle;

namespace GarageLog.Application.Interfaces.Services;

public interface IVehicleService
{
    Task<VehicleResponse> CreateVehicleAsync(CreateVehicleRequest request, int userId);

    Task<VehicleResponse?> GetVehicleAsync(int id, int userId);

    Task<IEnumerable<VehicleResponse>> GetVehiclesAsync(int userId);

    Task<VehicleResponse?> UpdateVehicleAsync(int id, UpdateVehicleRequest request, int userId);

    Task<bool> DeleteVehicleAsync(int id, int userId);
}
