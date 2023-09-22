using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using LightHouseReports.Common;
using LightHouseReports.Core;
using LightHouseReports.Data;
using LightHouseReports.UI;

namespace LightHouseReports.Desktop;

public partial class Application : Form
{
    public Application(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        blazorWebView1.HostPage = @"wwwroot\index.html";
        blazorWebView1.Services = serviceProvider;
        blazorWebView1.RootComponents.Add<App>("#app");
    }
}