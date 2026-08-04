using GarageLog.Core.Entities;

namespace GarageLog.Application.Interfaces.Repositories;

public interface IServiceRecordRepository
{
    Task<ServiceRecord?> GetByIdAsync(int vehicleId, int serviceRecordId);

    Task<IEnumerable<ServiceRecord>> GetAllByVehicleIdAsync(int vehicleId);

    Task<ServiceRecord?> GetPreviousRecordAsync(int vehicleId, DateTime serviceDate);

    Task<ServiceRecord?> GetNextRecordAsync(int vehicleId, DateTime serviceDate);

    void Delete(ServiceRecord serviceRecord);
}
