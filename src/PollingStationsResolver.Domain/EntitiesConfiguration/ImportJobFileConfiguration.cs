using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Domain.EntitiesConfiguration;

internal class ImportJobFileConfiguration : IEntityTypeConfiguration<ImportJobFile>
{
    public void Configure(EntityTypeBuilder<ImportJobFile> builder)
    {
        builder
            .Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()");
    }
}
