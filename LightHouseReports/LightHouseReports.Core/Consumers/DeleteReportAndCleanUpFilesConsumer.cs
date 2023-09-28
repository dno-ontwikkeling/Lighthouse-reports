using LightHouseReports.Common.Mediator;
using LightHouseReports.Core.Interfaces;
using LightHouseReports.Data.Interfaces;
using LightHouseReports.Data.Interfaces.Models;
using MassTransit.Mediator;

namespace LightHouseReports.Core.Consumers;

public class DeleteReportAndCleanUpFilesConsumer : CommandRequestConsumer<DeleteReportAndCleanUpFiles>
{
    private readonly IMediator _mediator;

    public DeleteReportAndCleanUpFilesConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected override async Task Consume(DeleteReportAndCleanUpFiles message, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(new DeleteReportDataModel(message.ReportIdGuid), cancellationToken);
            Directory.Delete("./Reports/" + message.ReportIdGuid, true);
        }
        catch (Exception e)
        {
        }
    }
}