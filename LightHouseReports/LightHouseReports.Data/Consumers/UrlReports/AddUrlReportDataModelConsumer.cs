using LightHouseReports.Common.Mediator;
using LightHouseReports.Data.Interfaces;
using LightHouseReports.UI.Interfaces.Events;
using MassTransit.Mediator;
using Microsoft.EntityFrameworkCore;

namespace LightHouseReports.Data.Consumers.UrlReports;

public class AddUrlReportDataModelConsumer : CommandRequestConsumer<AddUrlReportDataModel>
{
    private readonly AppContext _context;
    private readonly IMediator _mediator;

    public AddUrlReportDataModelConsumer(AppContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    protected override async Task Consume(AddUrlReportDataModel message, CancellationToken cancellationToken)
    {
        try
        {
            var website = await _context.Websites.FirstOrDefaultAsync(x => x.Id == message.ReportDataModel.Report.WebsiteDataModel.Id, cancellationToken);
            if (website is not null) message.ReportDataModel.Report.WebsiteDataModel = website;

            var report = await _context.WebsitesReport.FirstOrDefaultAsync(x => x.Id == message.ReportDataModel.Report.Id, cancellationToken);
            if (report is not null) message.ReportDataModel.Report = report;

            _context.UrlReports.Add(message.ReportDataModel);
            await _context.SaveChangesAsync(cancellationToken);
            await _mediator.Send(new EventMessageCommand(new ReportUpdate()), cancellationToken);
        }
        catch (Exception e)
        {
            // ignored
        }
    }
}