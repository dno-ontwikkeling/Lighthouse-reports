using LightHouseReports.Common.Mediator;
using LightHouseReports.Data.Interfaces;
using LightHouseReports.UI.Interfaces.Events;
using MassTransit.Mediator;

namespace LightHouseReports.Data.Consumers.Website;

public class AddWebsiteModelConsumer : CommandRequestConsumer<AddWebsiteModel>
{
    private readonly AppContext _context;
    private readonly IMediator _mediator;

    public AddWebsiteModelConsumer(AppContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    protected override async Task Consume(AddWebsiteModel message, CancellationToken cancellationToken)
    {
        try
        {
            _context.Websites.Add(message.Website);
            await _context.SaveChangesAsync(cancellationToken);
            await _mediator.Send(new EventMessageCommand(new WebsitesUpdate()), cancellationToken);
        }
        catch (Exception)
        {
            // ignored
        }
    }
}