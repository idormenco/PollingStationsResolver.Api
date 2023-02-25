using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PollingStationsResolver.Domain.Entities;

namespace PollingStationsResolver.Domain.EntitiesConfiguration;

internal class ResolvedAddressConfiguration : IEntityTypeConfiguration<ResolvedAddress>
{
    public void Configure(EntityTypeBuilder<ResolvedAddress> builder)
    {
        builder
            .Property(e => e.Id)
            .IsRequired();

        builder.HasIndex(e => e.Id);
    }
}
