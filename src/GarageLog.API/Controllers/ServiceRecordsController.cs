using GarageLog.API.Extensions;
using GarageLog.Application.DTOs.ServiceRecord;
using GarageLog.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageLog.API.Controllers;

[Authorize]
[ApiController]
[Route("api/vehicles/{vehicleId:int}/service-records")]
public class ServiceRecordsController(IServiceRecordService serviceRecordService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetServiceRecords(int vehicleId)
    {
        int userId = this.GetUserId();

        IEnumerable<ServiceRecordResponse>? records = await serviceRecordService.GetAllAsync(
            vehicleId,
            userId
        );

        if (records is null)
        {
            return NotFound();
        }

        return Ok(records);
    }

    [HttpGet("{serviceRecordId:int}")]
    public async Task<IActionResult> GetServiceRecord(int vehicleId, int serviceRecordId)
    {
        int userId = this.GetUserId();

        ServiceRecordResponse? record = await serviceRecordService.GetByIdAsync(
            vehicleId,
            serviceRecordId,
            userId
        );

        if (record is null)
        {
            return NotFound();
        }

        return Ok(record);
    }

    [HttpPost]
    public async Task<IActionResult> CreateServiceRecord(
        int vehicleId,
        CreateServiceRecordRequest request
    )
    {
        int userId = this.GetUserId();

        ServiceRecordResponse? record = await serviceRecordService.CreateAsync(
            vehicleId,
            userId,
            request
        );

        if (record is null)
        {
            return NotFound();
        }

        return CreatedAtAction(
            nameof(GetServiceRecord),
            new { vehicleId, serviceRecordId = record.Id },
            record
        );
    }

    [HttpDelete("{serviceRecordId:int}")]
    public async Task<IActionResult> DeleteServiceRecord(int vehicleId, int serviceRecordId)
    {
        int userId = this.GetUserId();

        bool deleted = await serviceRecordService.DeleteAsync(vehicleId, serviceRecordId, userId);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
