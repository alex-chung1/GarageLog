using GarageLog.Application.DTOs.ServiceType;
using GarageLog.Application.Interfaces.Repositories;
using GarageLog.Application.Interfaces.Services;

namespace GarageLog.Application.Services;

public class ServiceTypeService(IServiceTypeRepository repository) : IServiceTypeService
{
    public async Task<List<ServiceTypeResponse>> GetAllAsync()
    {
        var serviceTypes = await repository.GetAllAsync();

        return serviceTypes
            .Select(st => new ServiceTypeResponse
            {
                Id = st.Id,
                Name = st.Name,
                Category = st.Category,
            })
            .ToList();
    }
}
