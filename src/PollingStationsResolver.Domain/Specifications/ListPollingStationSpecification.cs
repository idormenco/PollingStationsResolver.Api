using Ardalis.Specification;
using PollingStationsResolver.Domain.Entities.PollingStationAggregate;

namespace PollingStationsResolver.Domain.Specifications;

public class ListPollingStationSpecification : Specification<PollingStation>
{
    public ListPollingStationSpecification(Pagination pagination, bool includeChildren = false)
    {
        Query.Skip(PaginationHelper.CalculateSkip(pagination))
            .Take(PaginationHelper.CalculateTake(pagination));

        if (includeChildren)
        {
            Query.Include(x => x.AssignedAddresses);
        }
    }
}