using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text.Json;
using System.Text.Json.Nodes;
using FluentResults;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Core.Interfaces;
using LightHouseReports.Core.Interfaces.Models;
using LightHouseReports.Core.Services;
using LightHouseReports.Data.Interfaces;
using LightHouseReports.Data.Interfaces.Models;
using MassTransit.Mediator;
using Microsoft.Extensions.Logging;
using Microsoft.PowerShell;

namespace LightHouseReports.Core.Consumers;

public class RunLighthouseWebsiteReportConsumer : DataRequestConsumer<RunLighthouseWebsiteReport, Result>
{
    private readonly IMediator _mediator;
    private readonly IWebsiteStateService _websiteStateService;
    private readonly ILogger<RunLighthouseWebsiteReportConsumer> _logger;

    public RunLighthouseWebsiteReportConsumer(IMediator mediator, IWebsiteStateService websiteStateService, ILogger<RunLighthouseWebsiteReportConsumer> logger)
    {
        _mediator = mediator;
        _websiteStateService = websiteStateService;
        _logger = logger;
    }

    protected override async Task<Result> Consume(RunLighthouseWebsiteReport message, CancellationToken cancellationToken)
    {
        var websiteResult = await _mediator.Request<GetWebsiteDataModel, Result<WebsiteDataModel>>(new GetWebsiteDataModel(message.WebsiteId), cancellationToken);
        if (websiteResult.IsFailed) return Result.Fail("Failed to load website model");
        var timeStamp = DateTimeOffset.UtcNow;
        var website = websiteResult.Value;
        website.LastRun = timeStamp;
        var report = new ReportDataModel
        {
            Id = Guid.NewGuid(),
            TimeStamp = timeStamp,
            WebsiteDataModel = website
        };
        try
        {
            var sitemapResult = await _mediator.Request<GetSitemapCoreModel, Result<SitemapCoreModel>>(new GetSitemapCoreModel(website.WebisteUrl), cancellationToken);
            if (sitemapResult.IsFailed) return sitemapResult.ToResult();
            var sitemap = sitemapResult.Value;

            website.FoundUrls = sitemap.Locs.Count;
            await _mediator.Send(new UpdateWebsiteDataModel(website), cancellationToken);
            await _websiteStateService.AddOrUpdateWebsiteProgress(website.Id, new ProgressCoreModel() { Done = 0, Total = sitemap.Locs.Count });

            var progress = 0;
            foreach (var loc in sitemap.Locs)
            {
                if (loc.Adres != null)
                {
                    var urlReport = new UrlReportDataModel(loc.Adres, report);

                    //Create directory for reports
                    var dir = $"./Reports/{report.Id}/{urlReport.Id}/";
                    Directory.CreateDirectory(dir);

                    // Create an initial default session state.
                    var initialSessionState = InitialSessionState.CreateDefault2();
                    initialSessionState.ExecutionPolicy = ExecutionPolicy.Bypass;

                    //Run light house script
                    var ps = PowerShell.Create(initialSessionState);
                    await ps.AddCommand($"./Scripts/lighthouseCommand.ps1")
                        .AddParameter("url", loc.Adres)
                        .AddParameter("folder", dir)
                        .InvokeAsync();

                    //Fetch result of report files
                    var jsonResultDesktop = JsonSerializer.Deserialize<JsonNode>(await File.ReadAllTextAsync($"{dir}desktop.report.json", cancellationToken));
                    urlReport.Results.Add(CreateLighthouseResult(jsonResultDesktop, Preset.Desktop));
                    var jsonResultPhone = JsonSerializer.Deserialize<JsonNode>(await File.ReadAllTextAsync($"{dir}phone.report.json", cancellationToken));
                    urlReport.Results.Add(CreateLighthouseResult(jsonResultPhone, Preset.Phone));

                    //Adding Url report results to database
                    await _mediator.Send(new AddUrlReportDataModel(urlReport), cancellationToken);
                }

                progress++;
                await _websiteStateService.AddOrUpdateWebsiteProgress(website.Id, new ProgressCoreModel() { Done = progress, Total = sitemap.Locs.Count });
            }

            await _mediator.Send(new UpdateWebsiteDataModel(website), cancellationToken);

            return Result.Ok();
        }
        catch (Exception e)
        {
            await _websiteStateService.AddOrUpdateWebsiteProgress(website.Id, new ProgressCoreModel() { Done = 0, Total = 0 });
            _logger.LogError(e, "Failed to run script");
            return Result.Fail(e.GetBaseException().Message);
        }
    }

    private static LighthouseResultDataModel CreateLighthouseResult(JsonNode? jsonObj, Preset preset)
    {
        var performance = (int)((jsonObj?["categories"]?["performance"]?["score"]?.GetValue<decimal>() ?? 0) * 100);
        var accessibility = (int)((jsonObj?["categories"]?["accessibility"]?["score"]?.GetValue<decimal>() ?? 0) * 100);
        var bestPractices = (int)((jsonObj?["categories"]?["best-practices"]?["score"]?.GetValue<decimal>() ?? 0) * 100);
        var seo = (int)((jsonObj?["categories"]?["seo"]?["score"]?.GetValue<decimal>() ?? 0) * 100);
        var lightHouseResultDesktop = new LighthouseResultDataModel(Guid.NewGuid(), preset, performance, accessibility, bestPractices, seo);
        return lightHouseResultDesktop;
    }
}