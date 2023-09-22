using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LightHouseReports.Common;

public static class Configuration
{
    public static IServiceCollection ConfigureCommon(this IServiceCollection services, IConfiguration config)
    {
        return services;
    }

    public static void ConfigureMediatorForCommonLibrary(this IMediatorRegistrationConfigurator options)
    {
        options.AddConsumers(typeof(Configuration).Assembly);
    }
}