using LightHouseReports.Data.Configurations;
using LightHouseReports.Data.Interfaces.Models;
using Microsoft.EntityFrameworkCore;

namespace LightHouseReports.Data;

public class AppContext : DbContext
{
    public DbSet<WebsiteDataModel> Websites { get; set; }
    public DbSet<ReportDataModel> WebsitesReport { get; set; }
    public DbSet<UrlReportDataModel> UrlReports { get; set; }

    public AppContext(DbContextOptions options) : base(options)
    {
        Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppContext).Assembly);
    }
}