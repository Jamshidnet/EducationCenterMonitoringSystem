using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MonitoringSystem.Application;

public static  class ConfigurationService
{
    public static bool IsBeeline(this string phoneNumber)
    {
        return phoneNumber.Substring(4, 2) == "90"
            || phoneNumber.Substring(4, 2) == "91";
    }
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}
