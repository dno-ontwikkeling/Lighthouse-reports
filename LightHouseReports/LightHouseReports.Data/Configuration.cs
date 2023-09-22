using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LightHouseReports.Data;

public static class Configuration
{
    public static IServiceCollection ConfigureData(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppContext>(options => { options.UseSqlite(config.GetConnectionString(nameof(AppContext))); });
        return services;
    }

    public static void ConfigureMediatorForDataLibrary(this IMediatorRegistrationConfigurator options)
    {
        options.AddConsumers(typeof(Configuration).Assembly);
    }
}