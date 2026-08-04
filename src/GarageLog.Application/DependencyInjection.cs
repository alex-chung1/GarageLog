using GarageLog.Application.Interfaces.Services;
using GarageLog.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GarageLog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IServiceRecordService, ServiceRecordService>();
        return services;
    }
}
