namespace LightHouseReports.Data.Interfaces.Models;

public class WebsiteDataModel
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public DateTimeOffset? LastRun { get; set; }
    public int FoundUrls { get; set; }
    public bool IsArchived { get; set; } = false;

    public WebsiteDataModel()
    {
    }

    public WebsiteDataModel(Guid id, string url, int foundUrls, DateTimeOffset? lastRun = null)
    {
        Id = id;
        Url = url;
        FoundUrls = foundUrls;
        LastRun = lastRun;
    }
}