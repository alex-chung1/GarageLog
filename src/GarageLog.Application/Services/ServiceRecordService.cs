using GarageLog.Application.DTOs.ServiceRecord;
using GarageLog.Application.Interfaces;
using GarageLog.Application.Interfaces.Repositories;
using GarageLog.Application.Interfaces.Services;
using GarageLog.Core.Entities;

namespace GarageLog.Application.Services;

public class ServiceRecordService(
    IVehicleRepository vehicleRepository,
    IServiceRecordRepository serviceRecordRepository,
    IServiceTypeRepository serviceTypeRepository,
    IUnitOfWork unitOfWork
) : IServiceRecordService
{
    public async Task<ServiceRecordResponse?> CreateAsync(
        int vehicleId,
        int userId,
        CreateServiceRecordRequest request
    )
    {
        if (request.Items.Count == 0)
            throw new InvalidOperationException(
                "A service record must contain at least one service item."
            );

        var vehicle = await vehicleRepository.GetByIdAsync(vehicleId, userId);

        if (vehicle is null)
            return null;

        var previous = await serviceRecordRepository.GetPreviousRecordAsync(
            vehicle.Id,
            request.ServiceDate
        );

        var next = await serviceRecordRepository.GetNextRecordAsync(
            vehicle.Id,
            request.ServiceDate
        );

        vehicle.ValidateHistoricalMileage(request.Mileage, previous, next);

        var serviceRecord = vehicle.AddServiceRecord(
            request.ServiceDate,
            request.Mileage,
            request.IsSelfService,
            request.TotalCost,
            request.ShopName,
            request.Notes
        );

        foreach (var itemRequest in request.Items)
        {
            var serviceType = await serviceTypeRepository.GetByIdAsync(itemRequest.ServiceTypeId);

            if (serviceType is null)
                throw new InvalidOperationException(
                    $"Service type {itemRequest.ServiceTypeId} not found."
                );

            serviceRecord.AddServiceItem(serviceType, itemRequest.CustomName);
        }

        await unitOfWork.SaveChangesAsync();

        return MapToResponse(serviceRecord);
    }

    public async Task<ServiceRecordResponse?> GetByIdAsync(
        int vehicleId,
        int serviceRecordId,
        int userId
    )
    {
        var vehicle = await vehicleRepository.GetByIdAsync(vehicleId, userId);

        if (vehicle is null)
            return null;

        var serviceRecord = await serviceRecordRepository.GetByIdAsync(vehicle.Id, serviceRecordId);

        return serviceRecord is null ? null : MapToResponse(serviceRecord);
    }

    public async Task<IEnumerable<ServiceRecordResponse>?> GetAllAsync(int vehicleId, int userId)
    {
        var vehicle = await vehicleRepository.GetByIdAsync(vehicleId, userId);

        if (vehicle is null)
            return null;

        var records = await serviceRecordRepository.GetAllByVehicleIdAsync(vehicleId);

        return records.Select(MapToResponse);
    }

    public async Task<ServiceRecordResponse?> UpdateAsync(
        int vehicleId,
        int serviceRecordId,
        int userId,
        UpdateServiceRecordRequest request
    )
    {
        var vehicle = await vehicleRepository.GetByIdAsync(vehicleId, userId);

        if (vehicle is null)
            return null;

        var serviceRecord = await serviceRecordRepository.GetByIdAsync(vehicleId, serviceRecordId);

        if (serviceRecord is null)
            return null;

        var previous = await serviceRecordRepository.GetPreviousRecordAsync(
            vehicle.Id,
            request.ServiceDate,
            excludeId: serviceRecord.Id
        );

        var next = await serviceRecordRepository.GetNextRecordAsync(
            vehicle.Id,
            request.ServiceDate,
            excludeId: serviceRecord.Id
        );

        vehicle.ValidateHistoricalMileage(request.Mileage, previous, next);

        serviceRecord.UpdateDetails(
            request.ServiceDate,
            request.Mileage,
            request.IsSelfService,
            request.TotalCost,
            request.ShopName,
            request.Notes
        );

        var newItems = new List<(ServiceType ServiceType, string? CustomName)>();

        foreach (var itemRequest in request.Items)
        {
            var serviceType = await serviceTypeRepository.GetByIdAsync(itemRequest.ServiceTypeId);

            if (serviceType is null)
                throw new InvalidOperationException(
                    $"Service type {itemRequest.ServiceTypeId} not found."
                );

            newItems.Add((serviceType, itemRequest.CustomName));
        }

        serviceRecord.ReplaceItems(newItems);

        await unitOfWork.SaveChangesAsync();

        return MapToResponse(serviceRecord);
    }

    public async Task<bool> DeleteAsync(int vehicleId, int serviceRecordId, int userId)
    {
        var vehicle = await vehicleRepository.GetByIdAsync(vehicleId, userId);

        if (vehicle is null)
            return false;

        var serviceRecord = await serviceRecordRepository.GetByIdAsync(vehicleId, serviceRecordId);

        if (serviceRecord is null)
            return false;

        serviceRecordRepository.Delete(serviceRecord);
        await unitOfWork.SaveChangesAsync();

        return true;
    }

    private static ServiceRecordResponse MapToResponse(ServiceRecord serviceRecord)
    {
        return new ServiceRecordResponse
        {
            Id = serviceRecord.Id,
            ServiceDate = serviceRecord.ServiceDate,
            Mileage = serviceRecord.Mileage,
            TotalCost = serviceRecord.TotalCost,
            IsSelfService = serviceRecord.IsSelfService,
            ShopName = serviceRecord.ShopName,
            Notes = serviceRecord.Notes,
            CreatedAt = serviceRecord.CreatedAt,
            Items = serviceRecord.Items.Select(MapToResponse),
        };
    }

    private static ServiceRecordItemResponse MapToResponse(ServiceRecordItem item)
    {
        return new ServiceRecordItemResponse
        {
            Id = item.Id,
            ServiceTypeId = item.ServiceTypeId,
            ServiceTypeName = item.ServiceType.Name,
            CustomName = item.CustomName,
        };
    }
}
