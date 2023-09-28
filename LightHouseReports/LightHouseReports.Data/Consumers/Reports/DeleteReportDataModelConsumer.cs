using FluentResults;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Data.Interfaces;
using LightHouseReports.UI.Interfaces.Events;
using MassTransit.Mediator;
using Microsoft.EntityFrameworkCore;

namespace LightHouseReports.Data.Consumers.Reports;

public class DeleteReportDataModelConsumer : CommandRequestConsumer<DeleteReportDataModel>
{
    private readonly AppContext _context;
    private readonly IMediator _mediator;

    public DeleteReportDataModelConsumer(AppContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    protected override async Task Consume(DeleteReportDataModel message, CancellationToken cancellationToken)
    {
        try
        {
            var report = await _context.WebsitesReport.FirstAsync(x => x.Id == message.Id, cancellationToken);
            if (report is not null)
            {
                var results = await _context.UrlReports.Where(x => x.Report.Id == report.Id).ToListAsync(cancellationToken);
                _context.UrlReports.RemoveRange(results);
                _context.WebsitesReport.Remove(report);
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