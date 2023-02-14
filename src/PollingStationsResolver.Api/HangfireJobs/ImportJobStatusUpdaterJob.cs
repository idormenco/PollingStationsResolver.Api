using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;

namespace PollingStationsResolver.Api.HangfireJobs;

public class ImportJobStatusUpdaterJob : IImportJobStatusUpdaterJob
{
    private readonly IRepository<ImportJob> _repository;
    private readonly IReadRepository<ImportedPollingStation> _importedPollingStationsRepository;

    public ImportJobStatusUpdaterJob(IRepository<ImportJob> repository, IReadRepository<ImportedPollingStation> importedPollingStationsRepository)
    {
        _repository = repository;
        _importedPollingStationsRepository = importedPollingStationsRepository;
    }

    public async Task Run(CancellationToken cancellationToken)
    {
        var importJob = await _repository.FirstOrDefaultAsync(new CurrentImportJobInProgressSpecification(), cancellationToken);
        if (importJob != null)
        {
            var hasUnprocessedPollingStations = await _importedPollingStationsRepository
                .AnyAsync(new GetImportedPollingStationByAddressStatusSpecification(importJob.Id, ResolvedAddressStatus.NotProcessed), cancellationToken);

            if (hasUnprocessedPollingStations)
            {
                return;
            }

            importJob.End();
            await _repository.UpdateAsync(importJob, cancellationToken);
        }
    }
}
