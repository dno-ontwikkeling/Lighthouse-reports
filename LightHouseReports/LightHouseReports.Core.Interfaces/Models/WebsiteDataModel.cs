using LightHouseReports.Data.Interfaces.Models;

namespace LightHouseReports.Core.Interfaces.Models;

public class WebsiteCoreModel
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public DateTimeOffset? LastRun { get; set; }
    public int FoundUrls { get; set; }
    public ProgressCoreModel? ProgressReport { get; set; }

    public WebsiteCoreModel()
    {
    }

    public WebsiteCoreModel(WebsiteDataModel dataModel, ProgressCoreModel? progressReport)
    {
        Id = dataModel.Id;
        Url = dataModel.Url;
        LastRun = dataModel.LastRun;
        FoundUrls = dataModel.FoundUrls;
        ProgressReport = progressReport;
    }

    public WebsiteCoreModel(string url, Guid id, DateTimeOffset? lastRun = null)
    {
        Url = url;
        Id = id;
        LastRun = lastRun;
    }
}