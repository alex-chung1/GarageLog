using GarageLog.Core.Entities;

namespace GarageLog.Application.Interfaces.Repositories;

public interface IServiceTypeRepository
{
    Task<ServiceType?> GetByIdAsync(int id);
    Task<List<ServiceType>> GetAllAsync();
}
