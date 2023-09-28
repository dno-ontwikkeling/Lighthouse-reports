namespace LightHouseReports.Data.Interfaces.Models;

public class LighthouseResultDataModel
{
    public LighthouseResultDataModel(Guid id, Preset preset, int performance, int accessibility, int bestPractices, int seo)
    {
        Id = id;
        Preset = preset;
        Performance = performance;
        Accessibility = accessibility;
        BestPractices = bestPractices;
        Seo = seo;
    }

    public Guid Id { get; set; }
    public Preset Preset { get; set; }
    public int Performance { get; set; }
    public int Accessibility { get; set; }
    public int BestPractices { get; set; }
    public int Seo { get; set; }
}