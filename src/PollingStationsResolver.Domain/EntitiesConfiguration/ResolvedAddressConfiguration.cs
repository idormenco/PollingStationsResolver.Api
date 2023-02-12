using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PollingStationsResolver.Domain.Entities;

namespace PollingStationsResolver.Domain.EntitiesConfiguration;

public class ResolvedAddressConfiguration : IEntityTypeConfiguration<ResolvedAddress>
{
    public void Configure(EntityTypeBuilder<ResolvedAddress> builder)
    {
        builder
            .Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()");
    }
}