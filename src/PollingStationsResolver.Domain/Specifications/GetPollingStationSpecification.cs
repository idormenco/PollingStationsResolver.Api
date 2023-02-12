using Ardalis.Specification;
using PollingStationsResolver.Domain.Entities.PollingStationAggregate;

namespace PollingStationsResolver.Domain.Specifications;

public class GetPollingStationSpecification : Specification<PollingStation>, ISingleResultSpecification<PollingStation>
{
    public GetPollingStationSpecification(Guid id)
    {
        Query.Where(x => x.Id == id).Include(x => x.AssignedAddresses);
    }
}