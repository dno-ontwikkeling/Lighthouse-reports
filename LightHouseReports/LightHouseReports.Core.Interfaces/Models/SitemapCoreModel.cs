using System.Xml.Serialization;

namespace LightHouseReports.Core.Interfaces.Models;

[XmlRoot(ElementName = "urlset")]
public class SitemapCoreModel
{
    [XmlElement(ElementName = "url")] public List<Loc> Locs { get; set; } = new();
}

[XmlRoot(ElementName = "url")]
public class Loc
{
    [XmlElement(ElementName = "loc")] public string? Adres { get; set; }
}