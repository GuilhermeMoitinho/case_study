using CaseLocaliza.Db.Configurations;
using CaseLocaliza.Models;
using Microsoft.EntityFrameworkCore;

namespace CaseLocaliza.Db.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Vehicle> Vehicles { get; set; }

    public DbSet<VehicleAudit> VehicleAudits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VehicleConfiguration).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
