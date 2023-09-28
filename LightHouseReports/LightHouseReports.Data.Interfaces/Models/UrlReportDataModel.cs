namespace LightHouseReports.Data.Interfaces.Models;

public class UrlReportDataModel
{
    public Guid Id { get; set; }
    public string Adres { get; set; } = string.Empty;
    public ReportDataModel Report { get; set; }
    public List<LighthouseResultDataModel> Results { get; set; } = new();
}