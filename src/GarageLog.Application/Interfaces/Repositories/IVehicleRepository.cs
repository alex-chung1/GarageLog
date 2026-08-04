using GarageLog.Core.Entities;

namespace GarageLog.Application.Interfaces.Repositories;

public interface IVehicleRepository
{
    Task<Vehicle?> GetByIdAsync(int vehicleId, int userId);

    Task<IEnumerable<Vehicle>> GetAllByUserIdAsync(int userId);

    void Add(Vehicle vehicle);

    void Delete(Vehicle vehicle);
}
