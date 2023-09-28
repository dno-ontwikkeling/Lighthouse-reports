namespace LightHouseReports.Data.Interfaces.Models;

public class ReportDataModel
{
    public Guid Id { get; set; }
    public WebsiteDataModel WebsiteDataModel { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
}

public enum Preset
{
    Desktop,
    Phone
}