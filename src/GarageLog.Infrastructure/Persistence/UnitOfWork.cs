using GarageLog.Application.Interfaces;
using GarageLog.Infrastructure.Persistence;

namespace GarageLog.Infrastructure.Persistence;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
