using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using LightHouseReports.UI.Options;

namespace LightHouseReports.UI;

public static class Configuration
{
    public static IServiceCollection ConfigureUserInferface(this IServiceCollection services, IConfiguration config)
    {
        services.ConfigureLocalization(config);
        services.AddEventAggregator(options => options.AutoRefresh = true);
        services.AddMudServices();
        return services;
    }

    public static void ConfigureMediatorForUILibrary(this IMediatorRegistrationConfigurator options)
    {
        options.AddConsumers(typeof(Configuration).Assembly);
    }

    private static IServiceCollection ConfigureLocalization(this IServiceCollection services, IConfiguration config)
    {
        services.AddOptions<SupportedCulturesOptions>().BindConfiguration(nameof(SupportedCulturesOptions));
        services.AddLocalization();
        return services;
    }
}