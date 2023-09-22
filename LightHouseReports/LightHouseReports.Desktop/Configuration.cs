using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LightHouseReports.Common;
using LightHouseReports.Core;
using LightHouseReports.Data;
using LightHouseReports.UI;
using Serilog;

namespace LightHouseReports.Desktop;

public static class Configuration
{
    public static IServiceCollection ConfigureDesktopApplication(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddSerilog((provider, configuration) => { configuration.ReadFrom.Configuration(config); });
        services.AddWindowsFormsBlazorWebView();
        services.AddMediator(options =>
        {
            options.ConfigureMediatorForCommonLibrary();
            options.ConfigureMediatorForUILibrary();
            options.ConfigureMediatorForCoreLibrary();
            options.ConfigureMediatorForDataLibrary();
        });


        return services;
    }
}