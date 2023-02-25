using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PollingStationsResolver.Domain.Entities.PollingStationAggregate;

namespace PollingStationsResolver.Domain.EntitiesConfiguration;

internal class PollingStationAddressConfiguration : IEntityTypeConfiguration<AssignedAddress>
{
    public void Configure(EntityTypeBuilder<AssignedAddress> builder)
    {
        builder
            .Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()");
    }
}
