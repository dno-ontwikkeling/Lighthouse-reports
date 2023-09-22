using LightHouseReports.Common.Mediator;
using LightHouseReports.Data.Interfaces;
using LightHouseReports.UI.Interfaces.Events;
using MassTransit.Mediator;

namespace LightHouseReports.Data.Consumers.Website;

public class UpdateWebsiteModelConsumer : CommandRequestConsumer<UpdateWebsiteModel>
{
    private readonly AppContext _context;
    private readonly IMediator _mediator;

    public UpdateWebsiteModelConsumer(AppContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    protected override async Task Consume(UpdateWebsiteModel message, CancellationToken cancellationToken)
    {
        try
        {
            if (_context.Websites.Any(x => x.Id == message.Website.Id))
            {
                _context.Websites.Update(message.Website);
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