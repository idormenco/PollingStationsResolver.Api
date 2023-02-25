using Ardalis.GuardClauses;
using PollingStationsResolver.Domain.Entities;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using PollingStationsResolver.Geocoding;
using PollingStationsResolver.Geocoding.Models;

namespace PollingStationsResolver.Api.HangfireJobs;

public class GeocodingJob : IGeocodingJob
{
    private readonly IRepository<ImportJob> _importJobRepository;
    private readonly IRepository<ImportedPollingStation> _repository;
    private readonly IRepository<ResolvedAddress> _resolvedAddressRepository;
    private readonly IGetAddressCoordinatesQuery _getAddressCoordinatesQuery;

    public GeocodingJob(IRepository<ImportJob> importJobRepository,
        IRepository<ImportedPollingStation> repository,
        IRepository<ResolvedAddress> resolvedAddressRepository,
        IGetAddressCoordinatesQuery getAddressCoordinatesQuery)
    {
        _importJobRepository = importJobRepository;
        _repository = repository;
        _resolvedAddressRepository = resolvedAddressRepository;
        _getAddressCoordinatesQuery = getAddressCoordinatesQuery;
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

        var getResolvedAddressSpecification = new GetResolvedAddressSpecification(importedPollingStation.County, importedPollingStation.Locality, importedPollingStation.Address);
        var existingResolvedAddress = await _resolvedAddressRepository.FirstOrDefaultAsync(getResolvedAddressSpecification, cancellationToken);

        importedPollingStation.MarkAsNotFound();

        if (existingResolvedAddress != null)
        {
            importedPollingStation.UpdateCoordinates(existingResolvedAddress.Latitude, existingResolvedAddress.Longitude);
        }
        else
        {
            var result = await _getAddressCoordinatesQuery.ExecuteAsync(importedPollingStation.County, importedPollingStation.Locality, importedPollingStation.Address, cancellationToken);

            if (result is LocationSearchResult.Found coordinates)
            {
                var (latitude, longitude) = coordinates;
                importedPollingStation.UpdateCoordinates(latitude, longitude);
                var resolvedAddress = new ResolvedAddress(importedPollingStation.County, importedPollingStation.Locality,  importedPollingStation.Address, latitude, longitude);
                await _resolvedAddressRepository.AddAsync(resolvedAddress, cancellationToken);
            }
        }

        await _repository.UpdateAsync(importedPollingStation, cancellationToken);
    }
}
