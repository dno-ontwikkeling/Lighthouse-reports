using System.Text.Json;
using LightHouseReports.Data.Interfaces.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LightHouseReports.Data.Configurations;

internal class WebsiteDataModelConfiguration : IEntityTypeConfiguration<WebsiteDataModel>
{
    public void Configure(EntityTypeBuilder<WebsiteDataModel> builder)
    {
        builder.Property(x => x.SiteMaps).HasConversion(
            list => JsonSerializer.Serialize(list, JsonSerializerOptions.Default),
            value => !string.IsNullOrWhiteSpace(value) ? JsonSerializer.Deserialize<List<string>>(value, JsonSerializerOptions.Default) ?? new List<string>() : new List<string>(),
            ValueComparer.CreateDefault(typeof(List<string>), false));
    }
}