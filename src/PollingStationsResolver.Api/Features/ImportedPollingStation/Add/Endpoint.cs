using PollingStationsResolver.Domain.Repository;
using ImportedPollingStationEntity = PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate.ImportedPollingStation;
using ImportJobEntity = PollingStationsResolver.Domain.Entities.ImportJobAggregate.ImportJob;

namespace PollingStationsResolver.Api.Features.ImportedPollingStation.Add;

public class Endpoint : EndpointWithMapping<AddImportedPollingStationRequest, ImportedPollingStationModel, ImportedPollingStationEntity>
{
    private readonly IRepository<ImportJobEntity> _importJobRepository;
    private readonly IRepository<ImportedPollingStationEntity> _repository;
    private readonly ResponseMapper _responseMapper;

    public Endpoint(IRepository<ImportedPollingStationEntity> repository, IRepository<ImportJobEntity> importJobRepository, ResponseMapper responseMapper)
    {
        _repository = repository;
        _importJobRepository = importJobRepository;
        _responseMapper = responseMapper;
    }

    public override void Configure()
    {
        Post("/import-job/{jobId:guid}/imported-polling-stations");
    }

    public override async Task HandleAsync(AddImportedPollingStationRequest request, CancellationToken ct)
    {
        var importJob = await _importJobRepository.GetByIdAsync(request.JobId, ct);
        if (importJob is not null)
        {
            var importedPollingStation = MapToEntity(request);
            var addedImportedPollingStation = await _repository.AddAsync(importedPollingStation, ct);

            await SendOkAsync(_responseMapper.FromEntity(addedImportedPollingStation), ct);
        }
        else
        {
            AddError($"Import job with id = '{request.JobId}' not found");
            await SendNotFoundAsync(ct);
        }
    }

    public override ImportedPollingStationEntity MapToEntity(AddImportedPollingStationRequest request)
    {
        var entity = new ImportedPollingStationEntity(
            request.PollingStationNumber,
            request.County,
            request.Locality,
            request.Address,
            request.Latitude,
            request.Longitude,
            request.ResolvedAddressStatus);

        foreach (var address in request.AssignedAddresses)
        {
            entity.AddAssignedAddress(address.Locality, address.StreetCode, address.Street, address.HouseNumbers, address.Remarks);
        }

        entity.AssignToJob(request.JobId);

        return entity;
    }
}
