using GarageLog.Application.Interfaces.Repositories;
using GarageLog.Core.Entities;
using GarageLog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GarageLog.Infrastructure.Repositories;

public class ServiceTypeRepository(AppDbContext context) : IServiceTypeRepository
{
    public async Task<ServiceType?> GetByIdAsync(int id)
    {
        return await context.ServiceTypes.FirstOrDefaultAsync(st => st.Id == id);
    }

    public async Task<List<ServiceType>> GetAllAsync()
    {
        return await context.ServiceTypes.OrderBy(st => st.Id).ThenBy(st => st.Name).ToListAsync();
    }
}
