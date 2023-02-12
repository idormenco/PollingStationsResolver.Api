using System.Collections.Immutable;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using PollingStationEntity = PollingStationsResolver.Domain.Entities.PollingStationAggregate.PollingStation;

namespace PollingStationsResolver.Api.Features.PollingStation.List;

public class Endpoint : Endpoint<Request, IImmutableList<PollingStationModel>, ResponseMapper>
{
    private readonly IRepository<PollingStationEntity> _repository;

    public Endpoint(IRepository<PollingStationEntity> repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Get("/polling-stations/");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        var entities = await _repository.ListAsync(new ListPollingStationSpecification(new Pagination(request.Page, request.PageSize)), ct);

        await SendOkAsync(entities.Select(Map.FromEntity).ToImmutableArray(), ct);
    }

}
