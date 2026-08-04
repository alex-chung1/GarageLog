using GarageLog.Application.DTOs.Vehicle;
using GarageLog.Core.Entities;

namespace GarageLog.Application.Interfaces.Repositories;

public interface IVehicleRepository
{
    Task<Vehicle?> GetByIdAsync(int vehicleId, int userId);
    Task<VehicleResponse?> GetDetailsByIdAsync(int vehicleId, int userId);

    Task<IEnumerable<VehicleResponse>> GetAllByUserIdAsync(int userId);

    void Add(Vehicle vehicle);

    void Delete(Vehicle vehicle);
}
