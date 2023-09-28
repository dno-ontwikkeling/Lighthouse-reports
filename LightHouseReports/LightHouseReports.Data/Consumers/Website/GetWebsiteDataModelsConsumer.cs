using FluentResults;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightHouseReports.Data.Consumers.Website;

public class GetWebsiteDataModelsConsumer : DataRequestConsumer<GetWebsiteDataModels, Result<List<Interfaces.Models.WebsiteDataModel>>>
{
    private readonly AppContext _context;

    public GetWebsiteDataModelsConsumer(AppContext context)
    {
        _context = context;
    }

    protected override async Task<Result<List<Interfaces.Models.WebsiteDataModel>>> Consume(GetWebsiteDataModels message, CancellationToken cancellationToken)
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