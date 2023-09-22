using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LightHouseReports.Core;

public static class Configuration
{
    public static IServiceCollection ConfigureCoreApplication(this IServiceCollection services, IConfiguration config)
    {
        return services;
    }

    public static void ConfigureMediatorForCoreLibrary(this IMediatorRegistrationConfigurator options)
    {
        options.AddConsumers(typeof(Configuration).Assembly);
    }
}