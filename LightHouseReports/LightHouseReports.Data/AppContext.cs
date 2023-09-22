using LightHouseReports.Data.Interfaces.Models;
using Microsoft.EntityFrameworkCore;

namespace LightHouseReports.Data;

public class AppContext : DbContext
{
    public DbSet<Website> Websites { get; set; }

    public AppContext(DbContextOptions options) : base(options)
    {
        Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}