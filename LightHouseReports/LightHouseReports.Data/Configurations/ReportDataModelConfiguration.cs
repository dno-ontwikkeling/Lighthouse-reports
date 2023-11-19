using LightHouseReports.Data.Interfaces.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LightHouseReports.Data.Configurations;

internal class ReportDataModelConfiguration : IEntityTypeConfiguration<ReportDataModel>
{
    public void Configure(EntityTypeBuilder<ReportDataModel> builder)
    {
        builder.Navigation(x => x.WebsiteDataModel).AutoInclude();
    }
}