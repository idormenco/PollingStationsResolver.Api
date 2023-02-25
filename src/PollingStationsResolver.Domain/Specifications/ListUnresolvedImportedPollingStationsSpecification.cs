using Ardalis.Specification;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Domain.Specifications;

public class ListUnresolvedImportedPollingStationsSpecification : Specification<ImportedPollingStation>
{
    public ListUnresolvedImportedPollingStationsSpecification(Guid jobId)
    {
        Query
            .Where(x => x.JobId == jobId)
            .Where(x => x.ResolvedAddressStatus == ResolvedAddressStatus.NotFound || x.ResolvedAddressStatus == ResolvedAddressStatus.NotProcessed);
    }
}
