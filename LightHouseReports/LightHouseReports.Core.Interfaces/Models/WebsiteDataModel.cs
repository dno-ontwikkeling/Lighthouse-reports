using LightHouseReports.Data.Interfaces.Models;

namespace LightHouseReports.Core.Interfaces.Models;

public class WebsiteCoreModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public string WebsiteUrl { get; set; }
    public List<string> SiteMaps { get; set; }
    public DateTimeOffset? LastRun { get; set; }
    public int FoundUrls { get; set; }
    public bool IsArchived { get; set; } = false;
    public ProgressCoreModel? ProgressReport { get; set; }

    public WebsiteCoreModel()
    {
    }

    public WebsiteCoreModel(WebsiteDataModel dataModel, ProgressCoreModel? progressReport)
    {
        Id = dataModel.Id;
        Name = dataModel.Name;
        WebsiteUrl = dataModel.WebisteUrl;
        SiteMaps = dataModel.SiteMaps;
        LastRun = dataModel.LastRun;
        FoundUrls = dataModel.FoundUrls;
        IsArchived = dataModel.IsArchived;
        ProgressReport = progressReport;
    }

    public WebsiteCoreModel(string websiteUrl, Guid id, DateTimeOffset? lastRun = null)
    {
        WebsiteUrl = websiteUrl;
        Id = id;
        LastRun = lastRun;
    }
}