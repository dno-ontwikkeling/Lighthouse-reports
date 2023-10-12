namespace LightHouseReports.Data.Interfaces.Models;

public class WebsiteDataModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string WebisteUrl { get; set; }
    public string[] SiteMaps { get; set; }
    public DateTimeOffset? LastRun { get; set; }
    public int FoundUrls { get; set; }
    public bool IsArchived { get; set; } = false;

    public WebsiteDataModel()
    {
    }

    public WebsiteDataModel(Guid id, string webisteUrl, int foundUrls, DateTimeOffset? lastRun = null)
    {
        Id = id;
        WebisteUrl = webisteUrl;
        FoundUrls = foundUrls;
        LastRun = lastRun;
    }
}