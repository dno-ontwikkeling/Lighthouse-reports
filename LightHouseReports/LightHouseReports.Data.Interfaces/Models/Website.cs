namespace LightHouseReports.Data.Interfaces.Models;

public class Website
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public DateTimeOffset? LastRun { get; set; }

    public Website()
    {
    }

    public Website(string url, Guid id, DateTimeOffset? lastRun = null)
    {
        Url = url;
        Id = id;
        LastRun = lastRun;
    }
}