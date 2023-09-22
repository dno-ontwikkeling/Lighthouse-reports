using System.Xml.Linq;

namespace LightHouseReports.UI.Options;

public class SupportedCulturesOptions
{
    public List<SupportedCulture> SupportedCultures { get; set; }
}

public class SupportedCulture
{
    public string Display { get; set; }
    public string Culture { get; set; }

    public override bool Equals(object? obj)
    {
        var other = obj as SupportedCulture;
        return other?.Culture == Culture;
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode()
    {
        return Culture.GetHashCode();
    }
}