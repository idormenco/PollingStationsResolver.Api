using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;

namespace PollingStationsResolver.Domain.EntitiesConfiguration;

internal class ImportedPollingStationConfiguration : IEntityTypeConfiguration<ImportedPollingStation>
{
    public void Configure(EntityTypeBuilder<ImportedPollingStation> builder)
    {
        builder
            .Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()");

        var navigation = builder.Metadata.FindNavigation(nameof(ImportedPollingStation.AssignedAddresses));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
