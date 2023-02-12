using Ardalis.Specification;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Domain.Specifications;

public class CurrentImportJobInProgressSpecification : Specification<ImportJob>, ISingleResultSpecification
{
    public CurrentImportJobInProgressSpecification()
    {
        Query.Where(x => x.JobStatus == ImportJobStatus.NotStarted
                         || x.JobStatus == ImportJobStatus.Started
                         || x.JobStatus == ImportJobStatus.Finished);
    }
}