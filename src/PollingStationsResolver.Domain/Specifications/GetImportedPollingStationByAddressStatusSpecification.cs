using Ardalis.Specification;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Domain.Specifications;

public class GetImportedPollingStationByAddressStatusSpecification : Specification<ImportedPollingStation>
{
    public GetImportedPollingStationByAddressStatusSpecification(ResolvedAddressStatus resolvedAddressStatus)
    {
        Query
            .Where(x => x.ResolvedAddressStatus == resolvedAddressStatus);
    }
}
