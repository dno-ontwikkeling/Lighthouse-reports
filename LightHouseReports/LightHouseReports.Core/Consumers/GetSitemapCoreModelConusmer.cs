using FluentResults;
using LightHouseReports.Common.Mediator;
using System.Xml.Serialization;
using Flurl.Http;
using System.Text.RegularExpressions;
using LightHouseReports.Core.Interfaces;
using LightHouseReports.Core.Interfaces.Models;

namespace LightHouseReports.Core.Consumers;

public class GetSitemapCoreModelConusmer : DataRequestConsumer<GetSitemapCoreModel, Result<SitemapCoreModel>>
{
    protected override async Task<Result<SitemapCoreModel>> Consume(GetSitemapCoreModel message, CancellationToken cancellationToken)
    {
        try
        {
            var websiteUrl = message.SitemapUrl;
            var baseUri = new UriBuilder(websiteUrl).Uri;
            var siteMapUri = new Uri(baseUri!, "sitemap.xml");
            var siteMapInXml = await siteMapUri.GetStringAsync(cancellationToken);
            var serializer = new XmlSerializer(typeof(SitemapCoreModel));
            using var reader = new StringReader(RemoveAttributesFromUrlset(siteMapInXml));
            var sitemap = (SitemapCoreModel)serializer.Deserialize(reader)!;
            return Result.Ok(sitemap);
        }
        catch (Exception e)
        {
            return Result.Fail(e.GetBaseException().Message);
        }
    }

    private string RemoveAttributesFromUrlset(string xml)
    {
        // Define the regular expression pattern to match the <urlset> element
        var pattern = @"<urlset\s[^>]*>";

        // Use Regex to find and replace the attributes in the <urlset> element
        var result = Regex.Replace(xml, pattern, "<urlset>", RegexOptions.Singleline);

        return result;
    }
}