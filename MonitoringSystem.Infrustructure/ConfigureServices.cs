using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Infrustructure.Persistence;
using MonitoringSystem.Infrustructure.Persistence.Interceptors;
using MonitoringSystem.Infrustructure.Services;

namespace MonitoringSystem.Infrustructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString: configuration.GetConnectionString("DbConnection"));
        });
      // services.AddDefaultIdentity<IdentityUser>

        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        services.AddTransient<IDateTime, DateTimeService>();
        
        services.AddTransient<IGuidGenerator, GuidGeneratorService>();
        
        return services;
    }
}