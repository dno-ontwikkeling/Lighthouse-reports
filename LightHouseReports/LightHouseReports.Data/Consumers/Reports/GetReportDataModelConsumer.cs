using FluentResults;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightHouseReports.Data.Consumers.Reports;

public class GetReportDataModelConsumer : DataRequestConsumer<GetUrlReportDataModel, Result<Interfaces.Models.UrlReportDataModel>>
{
    private readonly AppContext _context;

    public GetReportDataModelConsumer(AppContext context)
    {
        _context = context;
    }

    protected override async Task<Result<Interfaces.Models.UrlReportDataModel>> Consume(GetUrlReportDataModel message, CancellationToken cancellationToken)
    {
        try
        {
            var report = await _context.UrlReports
                .FirstAsync(x => x.Id == message.Id, cancellationToken);
            return report is null ? Result.Fail("Url results not found") : Result.Ok(report);
        }
        catch (Exception e)
        {
            return Result.Fail(e.GetBaseException().Message);
        }
    }
}