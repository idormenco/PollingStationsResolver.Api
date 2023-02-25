using PollingStationsResolver.Domain.Entities.ImportJobAggregate;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using ImportJobEntity = PollingStationsResolver.Domain.Entities.ImportJobAggregate.ImportJob;
using ImportedPollingStationEntity = PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate.ImportedPollingStation;
using PollingStationEntity = PollingStationsResolver.Domain.Entities.PollingStationAggregate.PollingStation;

namespace PollingStationsResolver.Api.Features.ImportJob.Complete;

public class Endpoint : EndpointWithoutRequest
{
    private readonly IRepository<ImportJobEntity> _importJobRepository;
    private readonly IRepository<PollingStationEntity> _pollingStationRepository;
    private readonly IRepository<ImportedPollingStationEntity> _importedPollingStationRepository;

    public Endpoint(IRepository<ImportedPollingStationEntity> importedPollingStationRepository,
        IRepository<ImportJobEntity> importJobRepository,
        IRepository<PollingStationEntity> pollingStationRepository)
    {
        _importedPollingStationRepository = importedPollingStationRepository;
        _importJobRepository = importJobRepository;
        _pollingStationRepository = pollingStationRepository;
    }

    public override void Configure()
    {
        Put("/import-job/{id:guid}/complete");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var jobId = Route<Guid>("id");
        var importJob = await _importJobRepository.GetByIdAsync(jobId, cancellationToken);
        if (importJob is not null)
        {
            if (importJob.JobStatus != ImportJobStatus.Finished)
            {
                ThrowError("Job status should be Finished to complete import!");
            }

            var hasUnresolvedAddresses = await _importedPollingStationRepository
                .AnyAsync(new ListImportedPollingStationsWithUnresolvedAddressSpecification(importJob.Id), cancellationToken);

            if (hasUnresolvedAddresses)
            {
                ThrowError("Cannot complete job with unresolved addresses!");
            }

            var importedPollingStations = await _importedPollingStationRepository.ListAsync(cancellationToken);
            var pollingStations = await _pollingStationRepository.ListAsync(cancellationToken);

            await _pollingStationRepository.DeleteRangeAsync(pollingStations, cancellationToken);
            pollingStations.Clear();

            foreach (var importedPollingStation in importedPollingStations)
            {
                var pollingStation = new PollingStationEntity(importedPollingStation.PollingStationNumber, importedPollingStation.County, importedPollingStation.Locality, importedPollingStation.Address, importedPollingStation.Latitude!.Value,
                      importedPollingStation.Longitude!.Value);

                foreach (var assignedAddress in importedPollingStation.AssignedAddresses)
                {
                    pollingStation.AddAssignedAddress(assignedAddress.Locality, assignedAddress.StreetCode, assignedAddress.Street, assignedAddress.HouseNumbers, assignedAddress.Remarks);
                }

                pollingStations.Add(pollingStation);
            }

            await _pollingStationRepository.AddRangeAsync(pollingStations, cancellationToken);

            await SendNoContentAsync(cancellationToken);
        }
        else
        {
            await SendNotFoundAsync(cancellationToken);
        }
    }
}
