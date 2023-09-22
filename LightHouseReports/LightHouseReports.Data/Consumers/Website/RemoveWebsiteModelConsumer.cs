using FluentResults;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Data.Interfaces;
using LightHouseReports.UI.Interfaces.Events;
using MassTransit.Mediator;

namespace LightHouseReports.Data.Consumers.Website;

public class RemoveWebsiteModelConsumer : CommandRequestConsumer<RemoveWebsiteModel>
{
    private readonly AppContext _context;
    private readonly IMediator _mediator;

    public RemoveWebsiteModelConsumer(AppContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    protected override async Task Consume(RemoveWebsiteModel message, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Request<GetWebsiteModel, Result<Interfaces.Models.Website>>(new GetWebsiteModel(message.Id), cancellationToken);
            if (result.IsSuccess)
            {
                _context.Websites.Remove(result.Value);
                await _context.SaveChangesAsync(cancellationToken);
                await _mediator.Send(new EventMessageCommand(new WebsitesUpdate()), cancellationToken);
            }
        }
        catch (Exception)
        {
            // ignored
        }
    }
}