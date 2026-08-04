using GarageLog.Application.DTOs.Vehicle;
using GarageLog.Application.Interfaces;
using GarageLog.Application.Interfaces.Repositories;
using GarageLog.Application.Interfaces.Services;
using GarageLog.Core.Entities;

namespace GarageLog.Application.Services;

public class VehicleService(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork)
    : IVehicleService
{
    public async Task<VehicleResponse> CreateVehicleAsync(CreateVehicleRequest request, int userId)
    {
        Vehicle vehicle = Vehicle.Create(
            userId,
            request.Type,
            request.Make,
            request.Model,
            request.Year,
            request.Vin
        );

        vehicleRepository.Add(vehicle);

        await unitOfWork.SaveChangesAsync();

        return MapToResponse(vehicle);
    }

    public async Task<VehicleResponse?> GetVehicleAsync(int id, int userId)
    {
        VehicleResponse? vehicle = await vehicleRepository.GetDetailsByIdAsync(id, userId);

        return vehicle;
    }

    public async Task<IEnumerable<VehicleResponse>> GetVehiclesAsync(int userId)
    {
        IEnumerable<VehicleResponse> vehicles = await vehicleRepository.GetAllByUserIdAsync(userId);

        return vehicles;
    }

    public async Task<VehicleResponse?> UpdateVehicleAsync(
        int id,
        UpdateVehicleRequest request,
        int userId
    )
    {
        Vehicle? vehicle = await vehicleRepository.GetByIdAsync(id, userId);

        if (vehicle is null)
        {
            return null;
        }

        vehicle.UpdateDetails(request.Type, request.Make, request.Model, request.Year, request.Vin);

        await unitOfWork.SaveChangesAsync();

        return MapToResponse(vehicle);
    }

    public async Task<bool> DeleteVehicleAsync(int id, int userId)
    {
        Vehicle? vehicle = await vehicleRepository.GetByIdAsync(id, userId);

        if (vehicle is null)
        {
            return false;
        }

        vehicleRepository.Delete(vehicle);

        await unitOfWork.SaveChangesAsync();

        return true;
    }

    private static VehicleResponse MapToResponse(Vehicle vehicle)
    {
        return new VehicleResponse
        {
            Id = vehicle.Id,
            Type = vehicle.Type,
            Make = vehicle.Make,
            Model = vehicle.Model,
            Year = vehicle.Year,
            Vin = vehicle.Vin,
            CreatedAt = vehicle.CreatedAt,
        };
    }
}
