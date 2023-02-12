using System.Collections.Immutable;
using Ardalis.GuardClauses;
using PollingStationsResolver.Api.Services.Geocoding;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;

namespace PollingStationsResolver.Api.Jobs;

public class GeocodingJob : IGeocodingJob
{
    private readonly IRepository<ImportJob> _importJobRepository;
    private readonly IRepository<ImportedPollingStation> _repository;
    private readonly IImmutableList<IGeocodingService> _addressLocationResolveServices;

    public GeocodingJob(IEnumerable<IGeocodingService> addressLocationResolveServices,
        IRepository<ImportJob> importJobRepository,
        IRepository<ImportedPollingStation> repository)
    {
        _importJobRepository = importJobRepository;
        _repository = repository;
        _addressLocationResolveServices = addressLocationResolveServices.ToImmutableList();
    }

    public async Task Run(Guid jobId, Guid pollingStationId, CancellationToken cancellationToken)
    {
        var importJob = await _importJobRepository.GetByIdAsync(jobId, cancellationToken);
        Guard.Against.Null(importJob);

        if (importJob!.JobStatus == ImportJobStatus.Canceled)
        {
            return;
        }

        var importedPollingStation = await _repository.FirstOrDefaultAsync(new GetImportedPollingStationSpecification(jobId, pollingStationId), cancellationToken);
        Guard.Against.Null(importedPollingStation);

        if (importedPollingStation.ResolvedAddressStatus == ResolvedAddressStatus.Success)
        {
            return;
        }

        foreach (var service in _addressLocationResolveServices)
        {
            var result = await service.FindCoordinatesAsync(importedPollingStation.County, importedPollingStation.Address);
            if (result.OperationStatus == ResolvedAddressStatus.Success)
            {
                importedPollingStation.UpdateCoordinates(result.Latitude, result.Longitude);
                await _repository.UpdateAsync(importedPollingStation, cancellationToken);
                break;
            }
        }
    }
}