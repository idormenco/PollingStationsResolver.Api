using PollingStationsResolver.Domain.Repository;
using PollingStationEntity = PollingStationsResolver.Domain.Entities.PollingStationAggregate.PollingStation;

namespace PollingStationsResolver.Api.Features.PollingStation.Delete;

public class Endpoint : Endpoint<Request>
{
    private readonly IRepository<PollingStationEntity> _repository;

    public Endpoint(IRepository<PollingStationEntity> repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Delete("/polling-stations/{id:guid}");
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        var pollingStation = await _repository.GetByIdAsync(request.Id, ct);
        if (pollingStation is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await _repository.DeleteAsync(pollingStation, ct);

        await SendNoContentAsync(ct);
    }

}