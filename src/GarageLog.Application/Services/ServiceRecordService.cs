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
        Vehicle? vehicle = await vehicleRepository.GetByIdAsync(vehicleId, userId);

        if (vehicle is null)
        {
            return null;
        }

        ServiceRecord? previous = await serviceRecordRepository.GetPreviousRecordAsync(
            vehicle.Id,
            request.ServiceDate
        );

        ServiceRecord? next = await serviceRecordRepository.GetNextRecordAsync(
            vehicle.Id,
            request.ServiceDate
        );

        vehicle.ValidateHistoricalMileage(request.Mileage, previous, next);

        ServiceRecord serviceRecord = vehicle.AddServiceRecord(
            request.ServiceDate,
            request.Mileage,
            request.IsSelfService,
            request.TotalCost,
            request.ShopName,
            request.Notes
        );

        foreach (CreateServiceRecordItemRequest itemRequest in request.Items)
        {
            ServiceType? serviceType = await serviceTypeRepository.GetByIdAsync(
                itemRequest.ServiceTypeId
            );

            if (serviceType is null)
            {
                throw new InvalidOperationException(
                    $"Service type {itemRequest.ServiceTypeId} not found."
                );
            }

            serviceRecord.AddServiceItem(serviceType, itemRequest.Quantity, itemRequest.CustomName);
        }

        if (
            next is null
            && (vehicle.CurrentMileage is null || request.Mileage > vehicle.CurrentMileage)
        )
        {
            vehicle.UpdateMileage(request.Mileage);
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
        Vehicle? vehicle = await vehicleRepository.GetByIdAsync(vehicleId, userId);

        if (vehicle is null)
        {
            return null;
        }

        ServiceRecord? serviceRecord = await serviceRecordRepository.GetByIdAsync(
            vehicle.Id,
            serviceRecordId
        );

        return serviceRecord is null ? null : MapToResponse(serviceRecord);
    }

    public async Task<IEnumerable<ServiceRecordResponse>?> GetAllAsync(int vehicleId, int userId)
    {
        Vehicle? vehicle = await vehicleRepository.GetByIdAsync(vehicleId, userId);

        if (vehicle is null)
        {
            return null;
        }

        IEnumerable<ServiceRecord> records = await serviceRecordRepository.GetAllByVehicleIdAsync(
            vehicleId
        );

        return records.Select(MapToResponse);
    }

    public async Task<bool> DeleteAsync(int vehicleId, int serviceRecordId, int userId)
    {
        Vehicle? vehicle = await vehicleRepository.GetByIdAsync(vehicleId, userId);

        if (vehicle is null)
        {
            return false;
        }

        ServiceRecord? serviceRecord = await serviceRecordRepository.GetByIdAsync(
            vehicleId,
            serviceRecordId
        );

        if (serviceRecord is null)
        {
            return false;
        }

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
            Quantity = item.Quantity,
            CustomName = item.CustomName,
        };
    }
}
