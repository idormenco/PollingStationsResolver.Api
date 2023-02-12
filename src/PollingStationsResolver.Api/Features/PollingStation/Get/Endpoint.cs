using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using PollingStationEntity = PollingStationsResolver.Domain.Entities.PollingStationAggregate.PollingStation;

namespace PollingStationsResolver.Api.Features.PollingStation.Get;

public class Endpoint : Endpoint<Request, PollingStationModel, ResponseMapper>
{
    private readonly IRepository<PollingStationEntity> _repository;

    public Endpoint(IRepository<PollingStationEntity> repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Get("/polling-stations/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        var entity = await _repository.FirstOrDefaultAsync(new GetPollingStationSpecification(request.Id), ct);
        if (entity is not null)
        {
            await SendOkAsync(Map.FromEntity(entity), ct);

        }
        else
        {
            await SendNotFoundAsync(ct);
        }
    }
}
