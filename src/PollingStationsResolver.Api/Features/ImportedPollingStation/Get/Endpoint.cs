using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using ImportedPollingStationEntity = PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate.ImportedPollingStation;

namespace PollingStationsResolver.Api.Features.ImportedPollingStation.Get;

public class Endpoint : Endpoint<Request, ImportedPollingStationModel, ResponseMapper>
{
    private readonly IRepository<ImportedPollingStationEntity> _repository;

    public Endpoint(IRepository<ImportedPollingStationEntity> repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Get("/import-job/{jobId:guid}/imported-polling-stations/{id:guid}");
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        var entity = await _repository.FirstOrDefaultAsync(new GetImportedPollingStationSpecification(request.JobId, request.Id), ct);
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
