using System.Collections.Immutable;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using ImportedPollingStationEntity = PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate.ImportedPollingStation;

namespace PollingStationsResolver.Api.Features.ImportedPollingStation.List;

public class Endpoint : Endpoint<Request, IImmutableList<ImportedPollingStationModel>, ResponseMapper>
{
    private readonly IRepository<ImportedPollingStationEntity> _repository;

    public Endpoint(IRepository<ImportedPollingStationEntity> repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Get("/import-job/{jobId:guid}/imported-polling-stations");
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        var entities = await _repository.ListAsync(new ListImportedPollingStationsSpecification(request.JobId, new Pagination(request.Page, request.PageSize)), ct);

        await SendOkAsync(entities.Select(Map.FromEntity).ToImmutableArray(), ct);
    }

}
