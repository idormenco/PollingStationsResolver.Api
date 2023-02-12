using Ardalis.Specification;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;

namespace PollingStationsResolver.Domain.Specifications;

public class ListImportedPollingStationsSpecification : Specification<ImportedPollingStation>
{
    public ListImportedPollingStationsSpecification(Guid jobId, Pagination? pagination = null, bool includeChildren = false)
    {
        Query
            .Where(x => x.JobId == jobId);

        if (pagination is not null)
        {
            Query
            .Skip(PaginationHelper.CalculateSkip(pagination))
                .Take(PaginationHelper.CalculateTake(pagination));
        }

        if (includeChildren)
        {
            Query
                .Include(x => x.AssignedAddresses);
        }
    }
}
