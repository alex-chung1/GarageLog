using GarageLog.Application.Interfaces.Repositories;
using GarageLog.Core.Entities;
using GarageLog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GarageLog.Infrastructure.Repositories;

public class ServiceRecordRepository(AppDbContext context) : IServiceRecordRepository
{
    public async Task<ServiceRecord?> GetByIdAsync(int vehicleId, int serviceRecordId)
    {
        return await context
            .ServiceRecords.Include(sr => sr.Items)
                .ThenInclude(i => i.ServiceType)
            .FirstOrDefaultAsync(sr => sr.VehicleId == vehicleId && sr.Id == serviceRecordId);
    }

    public async Task<IEnumerable<ServiceRecord>> GetAllByVehicleIdAsync(int vehicleId)
    {
        return await context
            .ServiceRecords.Where(sr => sr.VehicleId == vehicleId)
            .Include(sr => sr.Items)
                .ThenInclude(i => i.ServiceType)
            .OrderByDescending(sr => sr.ServiceDate)
            .ToListAsync();
    }

    public async Task<ServiceRecord?> GetPreviousRecordAsync(int vehicleId, DateTime serviceDate)
    {
        return await context
            .ServiceRecords.Where(sr => sr.VehicleId == vehicleId && sr.ServiceDate < serviceDate)
            .OrderByDescending(sr => sr.ServiceDate)
            .ThenByDescending(sr => sr.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<ServiceRecord?> GetNextRecordAsync(int vehicleId, DateTime serviceDate)
    {
        return await context
            .ServiceRecords.Where(sr => sr.VehicleId == vehicleId && sr.ServiceDate > serviceDate)
            .OrderBy(sr => sr.ServiceDate)
            .ThenBy(sr => sr.Id)
            .FirstOrDefaultAsync();
    }

    public void Delete(ServiceRecord serviceRecord)
    {
        context.ServiceRecords.Remove(serviceRecord);
    }
}
