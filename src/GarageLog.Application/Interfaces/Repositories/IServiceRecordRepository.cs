using GarageLog.Core.Entities;

namespace GarageLog.Application.Interfaces.Repositories;

public interface IServiceRecordRepository
{
    Task<ServiceRecord?> GetByIdAsync(int vehicleId, int serviceRecordId);

    Task<IEnumerable<ServiceRecord>> GetAllByVehicleIdAsync(int vehicleId);

    Task<ServiceRecord?> GetPreviousRecordAsync(
        int vehicleId,
        DateOnly serviceDate,
        int? excludeId = null
    );

    Task<ServiceRecord?> GetNextRecordAsync(
        int vehicleId,
        DateOnly serviceDate,
        int? excludeId = null
    );

    Task<ServiceRecord?> GetLatestRecordAsync(int vehicleId);

    void Delete(ServiceRecord serviceRecord);
}
