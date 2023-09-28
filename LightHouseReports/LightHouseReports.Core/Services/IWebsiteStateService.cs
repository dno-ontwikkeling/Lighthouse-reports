using LightHouseReports.Core.Interfaces.Models;

namespace LightHouseReports.Core.Services;

public interface IWebsiteStateService
{
    Task AddOrUpdateWebsiteProgress(Guid websiteId, ProgressCoreModel coreModel);
    ProgressCoreModel? GetWebsiteReportState(Guid websiteId);
}