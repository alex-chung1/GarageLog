using GarageLog.Application.DTOs.ServiceType;

namespace GarageLog.Application.Interfaces.Services;

public interface IServiceTypeService
{
    Task<List<ServiceTypeResponse>> GetAllAsync();
}
