using GarageLog.API.Extensions;
using GarageLog.Application.DTOs.Vehicle;
using GarageLog.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageLog.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class VehiclesController(IVehicleService vehicleService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetVehicles()
    {
        int userId = this.GetUserId();

        IEnumerable<VehicleResponse> vehicles = await vehicleService.GetVehiclesAsync(userId);

        return Ok(vehicles);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetVehicle(int id)
    {
        int userId = this.GetUserId();

        VehicleResponse? vehicle = await vehicleService.GetVehicleAsync(id, userId);

        if (vehicle is null)
        {
            return NotFound();
        }

        return Ok(vehicle);
    }

    [HttpPost]
    public async Task<IActionResult> CreateVehicle(CreateVehicleRequest request)
    {
        int userId = this.GetUserId();

        VehicleResponse vehicle = await vehicleService.CreateVehicleAsync(request, userId);

        return CreatedAtAction(nameof(GetVehicle), new { id = vehicle.Id }, vehicle);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateVehicle(int id, UpdateVehicleRequest request)
    {
        int userId = this.GetUserId();

        VehicleResponse? vehicle = await vehicleService.UpdateVehicleAsync(id, request, userId);

        if (vehicle is null)
        {
            return NotFound();
        }

        return Ok(vehicle);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteVehicle(int id)
    {
        int userId = this.GetUserId();

        bool deleted = await vehicleService.DeleteVehicleAsync(id, userId);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
