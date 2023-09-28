using System.Management.Automation;
using System.Text;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Core.Interfaces;

namespace LightHouseReports.Core.Consumers;

public class OpenLighthouseReportConsumer : CommandRequestConsumer<OpenLighthouseReport>
{
    protected override async Task Consume(OpenLighthouseReport message, CancellationToken cancellationToken)
    {
        try
        {
            var fullPath = Path.GetFullPath(message.path);
            var ps = await PowerShell.Create().AddCommand("start").AddArgument("chrome").AddArgument(FilePathToFileUrl(fullPath)).InvokeAsync();
        }
        catch (Exception e)
        {
        }
    }

    public static string FilePathToFileUrl(string filePath)
    {
        var uri = new StringBuilder();
        foreach (var v in filePath)
            if (v is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or >= '0' and <= '9' or '+' or '/' or ':' or '.' or '-' or '_' or '~' or > '\xFF')
                uri.Append(v);
            else if (v == Path.DirectorySeparatorChar || v == Path.AltDirectorySeparatorChar)
                uri.Append('/');
            else
                uri.Append($"%{(int)v:X2}");

        uri.Insert(0, uri is ['/', '/', ..] ? "file:" : "file:///"); // UNC path
        return uri.ToString();
    }
}