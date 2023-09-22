using FluentResults;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightHouseReports.Data.Consumers.Website;

public class GetWebsiteModelsConsumer : DataRequestConsumer<GetWebsiteModels, Result<List<LightHouseReports.Data.Interfaces.Models.Website>>>
{
    private readonly AppContext _context;

    public GetWebsiteModelsConsumer(AppContext context)
    {
        _context = context;
    }

    protected override async Task<Result<List<LightHouseReports.Data.Interfaces.Models.Website>>> Consume(GetWebsiteModels message, CancellationToken cancellationToken)
    {
        try
        {
            var websites = await _context.Websites.ToListAsync(cancellationToken);
            return Result.Ok(websites);
        }
        catch (Exception e)
        {
            return Result.Fail(e.GetBaseException().Message);
        }
    }
}