using System.Collections.Immutable;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using PollingStationEntity = PollingStationsResolver.Domain.Entities.PollingStationAggregate.PollingStation;

namespace PollingStationsResolver.Api.Features.PollingStation.Update;

public class Endpoint : Endpoint<UpdatePollingStationRequest, PollingStationModel>
{
    private readonly IRepository<PollingStationEntity> _repository;
    private readonly ResponseMapper _responseMapper;

    public Endpoint(IRepository<PollingStationEntity> repository, ResponseMapper responseMapper)
    {
        _repository = repository;
        _responseMapper = responseMapper;
    }

    public override void Configure()
    {
        Put("/polling-stations/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdatePollingStationRequest request, CancellationToken ct)
    {
        var entity = await _repository.FirstOrDefaultAsync(new GetPollingStationSpecification(request.Id), ct);
        if (entity is not null)
        {
            entity.UpdateDetails(request.PollingStationNumber, request.County, request.Locality, request.Address, request.Latitude, request.Longitude);

            var addressesToUpdate = request.AssignedAddresses
                .Where(x => x.Id != null)
                .ToDictionary(x => x.Id!.Value, y => y);

            var addressesToAdd = request.AssignedAddresses
                .Where(x => x.Id == null)
                .ToImmutableList();

            var addressesToDelete = entity.AssignedAddresses
                .Where(x => !addressesToUpdate.ContainsKey(x.Id))
                .ToImmutableArray();

            foreach (var (id, address) in addressesToUpdate)
            {
                entity.UpdateAddress(id, address.Locality, address.StreetCode, address.Street, address.HouseNumbers, address.Remarks);
            }

            foreach (var address in addressesToAdd)
            {
                entity.AddAssignedAddress(address.Locality, address.StreetCode, address.Street, address.HouseNumbers, address.Remarks);
            }

            foreach (var address in addressesToDelete)
            {
                entity.DeleteAddress(address.Id);
            }

            await _repository.UpdateAsync(entity, ct);
            await SendOkAsync(_responseMapper.FromEntity(entity), ct);
        }
        else
        {
            await SendNotFoundAsync(ct);
        }

    }
}
