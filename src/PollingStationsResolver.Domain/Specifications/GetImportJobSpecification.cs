using Ardalis.Specification;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Domain.Specifications;

public class GetImportJobSpecification : Specification<ImportJob>, ISingleResultSpecification
{
    public GetImportJobSpecification(Guid jobId, bool includeFile = false)
    {
        Query
            .Where(x => x.Id == jobId);

        if (includeFile)
        {
            Query.Include(x => x.File);
        }
    }
}