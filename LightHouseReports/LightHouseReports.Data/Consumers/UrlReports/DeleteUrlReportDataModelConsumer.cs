using FluentResults;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Data.Interfaces;
using LightHouseReports.UI.Interfaces.Events;
using MassTransit.Mediator;

namespace LightHouseReports.Data.Consumers.UrlReports;

public class DeleteUrlReportDataModelConsumer : CommandRequestConsumer<DeleteUrlReportDataModel>
{
    private readonly AppContext _context;
    private readonly IMediator _mediator;

    public DeleteUrlReportDataModelConsumer(AppContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    protected override async Task Consume(DeleteUrlReportDataModel message, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Request<GetUrlReportDataModel, Result<Interfaces.Models.UrlReportDataModel>>(new GetUrlReportDataModel(message.Id), cancellationToken);
            if (result.IsSuccess)
            {
                _context.UrlReports.Remove(result.Value);
                await _context.SaveChangesAsync(cancellationToken);
                await _mediator.Send(new EventMessageCommand(new ReportUpdate()), cancellationToken);
            }
        }
        catch (Exception)
        {
            // ignored
        }
    }
}