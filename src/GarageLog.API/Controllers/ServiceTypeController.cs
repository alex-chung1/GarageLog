using GarageLog.Application.DTOs.ServiceType;
using GarageLog.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageLog.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ServiceTypeController(IServiceTypeService serviceTypeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ServiceTypeResponse>>> GetAll()
    {
        var serviceTypes = await serviceTypeService.GetAllAsync();
        return Ok(serviceTypes);
    }
}
