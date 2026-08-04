using GarageLog.Application.Interfaces.Repositories;
using GarageLog.Core.Entities;
using GarageLog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GarageLog.Infrastructure.Repositories;

public class VehicleRepository(AppDbContext context) : IVehicleRepository
{
    public async Task<Vehicle?> GetByIdAsync(int vehicleId, int userId)
    {
        return await context.Vehicles.FirstOrDefaultAsync(v =>
            v.Id == vehicleId && v.UserId == userId
        );
    }

    public async Task<IEnumerable<Vehicle>> GetAllByUserIdAsync(int userId)
    {
        return await context
            .Vehicles.Where(v => v.UserId == userId)
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync();
    }

    public void Add(Vehicle vehicle)
    {
        context.Vehicles.Add(vehicle);
    }

    public void Delete(Vehicle vehicle)
    {
        context.Vehicles.Remove(vehicle);
    }
}
