using FluentResults;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Data.Interfaces;

namespace LightHouseReports.Data.Consumers.Website;

public class GetWebsiteDataModelConsumer : DataRequestConsumer<GetWebsiteDataModel, Result<Interfaces.Models.WebsiteDataModel>>
{
    private readonly AppContext _context;

    public GetWebsiteDataModelConsumer(AppContext context)
    {
        _context = context;
    }

    protected override async Task<Result<Interfaces.Models.WebsiteDataModel>> Consume(GetWebsiteDataModel message, CancellationToken cancellationToken)
    {
        try
        {
            var websites = await _context.Websites.FindAsync(message.Id);
            return websites is null ? Result.Fail("WebsiteDataModel not found") : Result.Ok(websites);
        }
        catch (Exception e)
        {
            return Result.Fail(e.GetBaseException().Message);
        }
    }
}