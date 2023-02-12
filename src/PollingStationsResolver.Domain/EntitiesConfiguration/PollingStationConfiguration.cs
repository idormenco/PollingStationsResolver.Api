using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PollingStationsResolver.Domain.Entities.PollingStationAggregate;

namespace PollingStationsResolver.Domain.EntitiesConfiguration;

public class PollingStationConfiguration : IEntityTypeConfiguration<PollingStation>
{
    public void Configure(EntityTypeBuilder<PollingStation> builder)
    {
        builder
            .Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()");

        var navigation = builder.Metadata.FindNavigation(nameof(PollingStation.AssignedAddresses));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}