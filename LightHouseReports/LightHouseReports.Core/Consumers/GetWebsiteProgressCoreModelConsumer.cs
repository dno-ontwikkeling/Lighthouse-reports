using FluentResults;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Core.Interfaces;
using LightHouseReports.Core.Interfaces.Models;
using LightHouseReports.Core.Services;
using Microsoft.Management.Infrastructure.CimCmdlets;

namespace LightHouseReports.Core.Consumers;

public class GetWebsiteProgressCoreModelConsumer : DataRequestConsumer<GetWebsiteProgressCoreModel, Result<ProgressCoreModel>>
{
    private readonly IWebsiteStateService _websiteStateService;

    public GetWebsiteProgressCoreModelConsumer(IWebsiteStateService websiteStateService)
    {
        _websiteStateService = websiteStateService;
    }

    protected override Task<Result<ProgressCoreModel>> Consume(GetWebsiteProgressCoreModel message, CancellationToken cancellationToken)
    {
        var progress = _websiteStateService.GetWebsiteReportState(message.WebsiteId);
        if (progress is null)
            return Task.FromResult((Result<ProgressCoreModel>)Result.Fail("No progress found"));
        else
            return Task.FromResult(Result.Ok(progress));
    }
}