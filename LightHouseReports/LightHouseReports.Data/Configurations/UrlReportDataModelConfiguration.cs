using LightHouseReports.Data.Interfaces.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace LightHouseReports.Data.Configurations;

internal class UrlReportDataModelConfiguration : IEntityTypeConfiguration<UrlReportDataModel>
{
    public void Configure(EntityTypeBuilder<UrlReportDataModel> builder)
    {
        builder.Navigation(x => x.Report).AutoInclude();
        builder.Navigation(x => x.Results).AutoInclude();
    }
}