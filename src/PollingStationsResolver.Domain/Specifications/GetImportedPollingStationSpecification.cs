using Ardalis.Specification;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;

namespace PollingStationsResolver.Domain.Specifications;

public class GetImportedPollingStationSpecification : Specification<ImportedPollingStation>, ISingleResultSpecification<ImportedPollingStation>
{
    public GetImportedPollingStationSpecification(Guid jobId, Guid id)
    {
        Query
            .Include(x => x.AssignedAddresses)
            .Where(x => x.Id == id && x.JobId == jobId);
    }
}