using GarageLog.Application.DTOs.ServiceRecord;

namespace GarageLog.Application.Interfaces.Services;

public interface IServiceRecordService
{
    Task<ServiceRecordResponse?> CreateAsync(
        int vehicleId,
        int userId,
        CreateServiceRecordRequest request
    );

    Task<ServiceRecordResponse?> GetByIdAsync(int vehicleId, int serviceRecordId, int userId);

    Task<IEnumerable<ServiceRecordResponse>?> GetAllAsync(int vehicleId, int userId);

    Task<ServiceRecordResponse?> UpdateAsync(
        int vehicleId,
        int serviceRecordId,
        int userId,
        UpdateServiceRecordRequest request
    );

    Task<bool> DeleteAsync(int vehicleId, int serviceRecordId, int userId);
}
