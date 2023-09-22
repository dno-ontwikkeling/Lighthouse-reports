using FluentResults;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Data.Interfaces;

namespace LightHouseReports.Data.Consumers.Website;

public class GetWebsiteModelConsumer : DataRequestConsumer<GetWebsiteModel, Result<LightHouseReports.Data.Interfaces.Models.Website>>
{
    private readonly AppContext _context;

    public GetWebsiteModelConsumer(AppContext context)
    {
        _context = context;
    }

    protected override async Task<Result<LightHouseReports.Data.Interfaces.Models.Website>> Consume(GetWebsiteModel message, CancellationToken cancellationToken)
    {
        try
        {
            var websites = await _context.Websites.FindAsync(message.Id);
            return websites is null ? Result.Fail("Website not found") : Result.Ok(websites);
        }
        catch (Exception e)
        {
            return Result.Fail(e.GetBaseException().Message);
        }
    }
}