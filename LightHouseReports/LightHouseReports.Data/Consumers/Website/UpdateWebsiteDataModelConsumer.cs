using LightHouseReports.Common.Mediator;
using LightHouseReports.Data.Interfaces;
using LightHouseReports.UI.Interfaces.Events;
using MassTransit.Mediator;

namespace LightHouseReports.Data.Consumers.Website;

public class UpdateWebsiteDataModelConsumer : CommandRequestConsumer<UpdateWebsiteDataModel>
{
    private readonly AppContext _context;
    private readonly IMediator _mediator;

    public UpdateWebsiteDataModelConsumer(AppContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    protected override async Task Consume(UpdateWebsiteDataModel message, CancellationToken cancellationToken)
    {
        try
        {
            if (_context.Websites.Any(x => x.Id == message.WebsiteDataModel.Id))
            {
                _context.Websites.Update(message.WebsiteDataModel);
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