using GarageLog.Application.DTOs.Vehicle;
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

    public async Task<VehicleResponse?> GetDetailsByIdAsync(int vehicleId, int userId)
    {
        return await context
            .Vehicles.Where(v => v.Id == vehicleId && v.UserId == userId)
            .Select(v => new VehicleResponse
            {
                Id = v.Id,
                Type = v.Type,
                Make = v.Make,
                Model = v.Model,
                Year = v.Year,
                Vin = v.Vin,
                CreatedAt = v.CreatedAt,
                LatestMileage = v
                    .ServiceRecords.OrderByDescending(sr => sr.ServiceDate)
                    .Select(sr => (int?)sr.Mileage)
                    .FirstOrDefault(),
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<VehicleResponse>> GetAllByUserIdAsync(int userId)
    {
        return await context
            .Vehicles.Where(v => v.UserId == userId)
            .OrderByDescending(v => v.CreatedAt)
            .Select(v => new VehicleResponse
            {
                Id = v.Id,
                Type = v.Type,
                Make = v.Make,
                Model = v.Model,
                Year = v.Year,
                Vin = v.Vin,
                CreatedAt = v.CreatedAt,
                LatestMileage = v
                    .ServiceRecords.OrderByDescending(sr => sr.ServiceDate)
                    .Select(sr => (int?)sr.Mileage)
                    .FirstOrDefault(),
            })
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
