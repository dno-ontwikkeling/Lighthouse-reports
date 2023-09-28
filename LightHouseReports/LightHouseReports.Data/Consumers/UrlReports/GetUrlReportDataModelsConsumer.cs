using FluentResults;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightHouseReports.Data.Consumers.UrlReports;

public class GetUrlReportDataModelsConsumer : DataRequestConsumer<GetUrlReportDataModels, Result<List<Interfaces.Models.UrlReportDataModel>>>
{
    private readonly AppContext _context;

    public GetUrlReportDataModelsConsumer(AppContext context)
    {
        _context = context;
    }

    protected override async Task<Result<List<Interfaces.Models.UrlReportDataModel>>> Consume(GetUrlReportDataModels message, CancellationToken cancellationToken)
    {
        try
        {
            var reports = await _context.UrlReports
                .ToListAsync(cancellationToken);
            return Result.Ok(reports);
        }
        catch (Exception e)
        {
            return Result.Fail(e.GetBaseException().Message);
        }
    }
}