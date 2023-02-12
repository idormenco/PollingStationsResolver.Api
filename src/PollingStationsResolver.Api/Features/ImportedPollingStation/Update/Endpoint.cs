using System.Collections.Immutable;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using ImportedPollingStationEntity = PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate.ImportedPollingStation;

namespace PollingStationsResolver.Api.Features.ImportedPollingStation.Update;

public class Endpoint : EndpointWithMapping<UpdateImportedPollingStationRequest, ImportedPollingStationModel, ImportedPollingStationEntity>
{
    private readonly IRepository<ImportedPollingStationEntity> _repository;
    private readonly ResponseMapper _responseMapper;

    public Endpoint(IRepository<ImportedPollingStationEntity> repository, ResponseMapper responseMapper)
    {
        _repository = repository;
        _responseMapper = responseMapper;
    }

    public override void Configure()
    {
        Put("/import-job/{jobId:guid}/imported-polling-stations");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateImportedPollingStationRequest request, CancellationToken ct)
    {
        var importedPollingStation = await _repository.FirstOrDefaultAsync(new GetImportedPollingStationSpecification(request.JobId, request.Id), ct);
        if (importedPollingStation is not null)
        {
            importedPollingStation.UpdateDetails(request.PollingStationNumber, request.County, request.Locality, request.Address, request.Latitude, request.Longitude, request.ResolvedAddressStatus);

            var addressesToUpdate = request.AssignedAddresses
                .Where(x => x.Id != null)
                .ToDictionary(x => x.Id!.Value, y => y);

            var addressesToAdd = request.AssignedAddresses
                .Where(x => x.Id == null)
                .ToImmutableList();

            var addressesToDelete = importedPollingStation.AssignedAddresses
                .Where(x => !addressesToUpdate.ContainsKey(x.Id))
                .ToImmutableList();

            foreach (var (id, address) in addressesToUpdate)
            {
                importedPollingStation.UpdateAddress(id, address.Locality, address.StreetCode, address.Street, address.HouseNumbers, address.Remarks);
            }

            foreach (var address in addressesToAdd)
            {
                importedPollingStation.AddAssignedAddress(address.Locality, address.StreetCode, address.Street, address.HouseNumbers, address.Remarks);
            }

            foreach (var address in addressesToDelete)
            {
                importedPollingStation.DeleteAddress(address.Id);
            }

            await _repository.UpdateAsync(importedPollingStation, ct);
            await SendOkAsync(_responseMapper.FromEntity(importedPollingStation), ct);
        }
        else
        {
            await SendNotFoundAsync(ct);
        }
    }
}
