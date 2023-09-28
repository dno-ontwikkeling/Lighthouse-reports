using LightHouseReports.Common.Mediator;
using LightHouseReports.Core.Interfaces;
using System.Management.Automation;
using System.Text.Json;
using System.Text.Json.Nodes;
using FluentResults;
using LightHouseReports.Data.Interfaces;
using LightHouseReports.Data.Interfaces.Models;
using MassTransit.Mediator;
using LightHouseReports.Core.Interfaces.Models;
using LightHouseReports.Core.Services;

namespace LightHouseReports.Core.Consumers;

public class RunLighthouseWebsiteReportConsumer : DataRequestConsumer<RunLighthouseWebsiteReport, Result>
{
    private readonly IMediator _mediator;
    private readonly IWebsiteStateService _websiteStateService;

    public RunLighthouseWebsiteReportConsumer(IMediator mediator, IWebsiteStateService websiteStateService)
    {
        _mediator = mediator;
        _websiteStateService = websiteStateService;
    }

    protected override async Task<Result> Consume(RunLighthouseWebsiteReport message, CancellationToken cancellationToken)
    {
        try
        {
            var websiteResult = await _mediator.Request<GetWebsiteDataModel, Result<WebsiteDataModel>>(new GetWebsiteDataModel(message.WebsiteId), cancellationToken);
            if (websiteResult.IsSuccess)
            {
                var timeStamp = DateTimeOffset.UtcNow;
                var website = websiteResult.Value;
                website.LastRun = timeStamp;
                var report = new ReportDataModel
                {
                    Id = Guid.NewGuid(),
                    TimeStamp = timeStamp,
                    WebsiteDataModel = website
                };

                var sitemapResult = await _mediator.Request<GetSitemapCoreModel, Result<SitemapCoreModel>>(new GetSitemapCoreModel(website.Url), cancellationToken);
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
                        var urlId = Guid.NewGuid();
                        var urlReport = new UrlReportDataModel()
                        {
                            Adres = loc.Adres,
                            Id = urlId,
                            Report = report
                        };

                        var dir = $"./Reports/{report.Id}/{urlId}/";
                        Directory.CreateDirectory(dir);

                        var ps = PowerShell.Create();

                        await ps.AddCommand("Set-ExecutionPolicy")
                            .AddParameter("ExecutionPolicy", "RemoteSigned")
                            .AddParameter("Scope", "LocalMachine")
                            .AddCommand(@"./Scripts/lighthouseCommand.ps1")
                            .AddParameter("url", loc.Adres)
                            .AddParameter("folder", dir)
                            .InvokeAsync();

                        var jsonResultDesktop = JsonSerializer.Deserialize<JsonNode>(await File.ReadAllTextAsync($"./Reports/{report.Id}/{urlId}/desktop.report.json", cancellationToken));
                        urlReport.Results.Add(CreateLighthouseResult(jsonResultDesktop, Preset.Desktop));

                        var jsonResultPhone = JsonSerializer.Deserialize<JsonNode>(await File.ReadAllTextAsync($"./Reports/{report.Id}/{urlId}/phone.report.json", cancellationToken));
                        urlReport.Results.Add(CreateLighthouseResult(jsonResultPhone, Preset.Phone));


                        await _mediator.Send(new AddUrlReportDataModel(urlReport), cancellationToken);
                    }

                    progress++;
                    await _websiteStateService.AddOrUpdateWebsiteProgress(website.Id, new ProgressCoreModel() { Done = progress, Total = sitemap.Locs.Count });
                }

                await _mediator.Send(new UpdateWebsiteDataModel(website), cancellationToken);
                ;
            }

            return Result.Ok();
        }
        catch (Exception e)
        {
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