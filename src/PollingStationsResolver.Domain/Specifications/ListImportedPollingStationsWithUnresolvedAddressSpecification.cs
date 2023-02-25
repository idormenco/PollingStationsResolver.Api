using Ardalis.Specification;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Domain.Specifications;

public class ListImportedPollingStationsWithUnresolvedAddressSpecification : Specification<ImportedPollingStation>
{
    public ListImportedPollingStationsWithUnresolvedAddressSpecification(Guid jobId)
    {
        Query
            .Where(x => x.JobId == jobId)
            .Where(x => x.ResolvedAddressStatus != ResolvedAddressStatus.Success);

    }
}
