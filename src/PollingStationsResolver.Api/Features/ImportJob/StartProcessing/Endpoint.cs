using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using ImportJobEntity = PollingStationsResolver.Domain.Entities.ImportJobAggregate.ImportJob;
using ImportedPollingStationEntity = PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate.ImportedPollingStation;
using Hangfire;
using PollingStationsResolver.Api.Jobs;

namespace PollingStationsResolver.Api.Features.ImportJob.StartProcessing;

public class Endpoint : EndpointWithoutRequest
{
    private readonly IRepository<ImportJobEntity> _importJobRepository;
    private readonly IRepository<ImportedPollingStationEntity> _repository;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public Endpoint(IRepository<ImportedPollingStationEntity> repository,
        IRepository<ImportJobEntity> importJobRepository,
        IBackgroundJobClient backgroundJobClient)
    {
        _repository = repository;
        _importJobRepository = importJobRepository;
        _backgroundJobClient = backgroundJobClient;
    }

    public override void Configure()
    {
        Put("/import-job/{id:guid}/start-processing");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var jobId = Route<Guid>("id");
        var importJob = await _importJobRepository.GetByIdAsync(jobId, ct);
        if (importJob is not null)
        {
            importJob.Start();
            await _importJobRepository.UpdateAsync(importJob, ct);

            var importedPollingStations = await _repository.ListAsync(new ListImportedPollingStationsSpecification(jobId), ct);

            foreach (var importedPollingStation in importedPollingStations)
            {
                _backgroundJobClient.Enqueue<IGeocodingJob>(x => x.Run(importedPollingStation.JobId, importedPollingStation.Id, CancellationToken.None));
            }

            await SendNoContentAsync(ct);
        }
        else
        {
            await SendNotFoundAsync(ct);
        }
    }
}
