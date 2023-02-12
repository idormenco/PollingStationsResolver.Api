using PollingStationsResolver.Domain.Repository;
using PollingStationEntity = PollingStationsResolver.Domain.Entities.PollingStationAggregate.PollingStation;

namespace PollingStationsResolver.Api.Features.PollingStation.Add;

public class Endpoint : EndpointWithMapping<AddPollingStationRequest, PollingStationModel, PollingStationEntity>
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
        Post("/polling-stations");
    }

    public override async Task HandleAsync(AddPollingStationRequest request, CancellationToken ct)
    {
        var pollingStation = MapToEntity(request);
        var addedPollingStation = await _repository.AddAsync(pollingStation, ct);

        await SendOkAsync(_responseMapper.FromEntity(addedPollingStation), ct);
    }

    public override PollingStationEntity MapToEntity(AddPollingStationRequest request)
    {
        var entity = new PollingStationEntity(request.PollingStationNumber,
            request.County,
            request.Locality,
            request.Address,
            request.Latitude,
            request.Longitude);

        foreach (var address in request.AssignedAddresses)
        {
            entity.AddAssignedAddress(address.Locality, address.StreetCode, address.Street, address.HouseNumbers, address.Remarks);
        }

        return entity;
    }
}
