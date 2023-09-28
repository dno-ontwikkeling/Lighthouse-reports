using LightHouseReports.Common.Mediator;
using LightHouseReports.Data.Interfaces;
using LightHouseReports.UI.Interfaces.Events;
using MassTransit.Mediator;

namespace LightHouseReports.Data.Consumers.Website;

public class AddWebsiteDataModelConsumer : CommandRequestConsumer<AddWebsiteDataModel>
{
    private readonly AppContext _context;
    private readonly IMediator _mediator;

    public AddWebsiteDataModelConsumer(AppContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    protected override async Task Consume(AddWebsiteDataModel message, CancellationToken cancellationToken)
    {
        try
        {
            _context.Websites.Add(message.WebsiteDataModel);
            await _context.SaveChangesAsync(cancellationToken);
            await _mediator.Send(new EventMessageCommand(new WebsitesUpdate()), cancellationToken);
        }
        catch (Exception)
        {
            // ignored
        }
    }
}