using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;

namespace PollingStationsResolver.Domain.EntitiesConfiguration;

public class ImportedPollingStationAddressConfiguration : IEntityTypeConfiguration<ImportedPollingStationAddress>
{
    public void Configure(EntityTypeBuilder<ImportedPollingStationAddress> builder)
    {
        builder
            .Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()");
    }
}