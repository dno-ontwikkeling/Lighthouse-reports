using LightHouseReports.Core.Interfaces.Models;
using LightHouseReports.UI.Interfaces.Events;
using MassTransit.Mediator;

namespace LightHouseReports.Core.Services;

public class WebsiteStateService : IWebsiteStateService
{
    private readonly Dictionary<Guid, ProgressCoreModel> _progressReports;
    private readonly IMediator _mediator;

    public WebsiteStateService(IMediator mediator)
    {
        _mediator = mediator;
        _progressReports = new Dictionary<Guid, ProgressCoreModel>();
    }

    public ProgressCoreModel? GetWebsiteReportState(Guid websiteId)
    {
        return _progressReports.TryGetValue(websiteId, out var report) ? report : null;
    }

    public async Task AddOrUpdateWebsiteProgress(Guid websiteId, ProgressCoreModel coreModel)
    {
        if (!_progressReports.ContainsKey(websiteId))
        {
            _progressReports.Add(websiteId, coreModel);
        }
        else
        {
            _progressReports[websiteId] = coreModel;
            if (coreModel.Done >= coreModel.Total) _progressReports.Remove(websiteId);
        }

        await _mediator.Send(new EventMessageCommand(new ReportProgressUpdate()));
    }
}