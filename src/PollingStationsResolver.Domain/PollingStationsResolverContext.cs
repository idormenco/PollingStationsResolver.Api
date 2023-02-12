using Microsoft.EntityFrameworkCore;
using PollingStationsResolver.Domain.Entities;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;
using PollingStationsResolver.Domain.Entities.PollingStationAggregate;
using PollingStationsResolver.Domain.EntitiesConfiguration;
using SmartEnum.EFCore;

namespace PollingStationsResolver.Domain;

public class PollingStationsResolverContext : DbContext
{
    public PollingStationsResolverContext(DbContextOptions<PollingStationsResolverContext> options) : base(options) { }

    public DbSet<PollingStation> PollingStations { get; init; }
    public DbSet<AssignedAddress> PollingStationAddresses { get; init; }
    public DbSet<ImportJob> ImportJobs { get; init; }
    public DbSet<ImportedPollingStation> ImportedPollingStations { get; init; }
    public DbSet<ImportedPollingStationAddress> ImportedPollingStationAddresses { get; init; }
    public DbSet<ResolvedAddress> ResolvedAddresses { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ConfigureSmartEnum();
        base.OnModelCreating(builder);

        builder.HasPostgresExtension("uuid-ossp");

        builder.ApplyConfiguration(new PollingStationConfiguration());
        builder.ApplyConfiguration(new PollingStationAddressConfiguration());
        builder.ApplyConfiguration(new ImportJobConfiguration());
        builder.ApplyConfiguration(new ImportJobFileConfiguration());

        builder.ApplyConfiguration(new ImportedPollingStationConfiguration());
        builder.ApplyConfiguration(new ImportedPollingStationAddressConfiguration());
        builder.ApplyConfiguration(new ResolvedAddressConfiguration());
    }

}