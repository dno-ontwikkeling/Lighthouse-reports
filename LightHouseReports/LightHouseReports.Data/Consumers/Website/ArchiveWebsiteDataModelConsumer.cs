using FluentResults;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Data.Interfaces;
using LightHouseReports.UI.Interfaces.Events;
using MassTransit.Mediator;

namespace LightHouseReports.Data.Consumers.Website;

public class ArchiveWebsiteDataModelConsumer : CommandRequestConsumer<ArchiveWebsiteDataModel>
{
    private readonly AppContext _context;
    private readonly IMediator _mediator;

    public ArchiveWebsiteDataModelConsumer(AppContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    protected override async Task Consume(ArchiveWebsiteDataModel message, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Request<GetWebsiteDataModel, Result<Interfaces.Models.WebsiteDataModel>>(new GetWebsiteDataModel(message.Id), cancellationToken);
            if (result.IsSuccess)
            {
                result.Value.IsArchived = true;
                _context.Websites.Update(result.Value);
                await _context.SaveChangesAsync(cancellationToken);
                await _mediator.Send(new EventMessageCommand(new WebsitesUpdate()), cancellationToken);
            }
        }
        catch (Exception e)
        {
            // ignored
        }
    }
}