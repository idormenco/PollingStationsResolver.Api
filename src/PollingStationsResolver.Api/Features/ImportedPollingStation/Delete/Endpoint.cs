using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using ImportedPollingStationEntity = PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate.ImportedPollingStation;

namespace PollingStationsResolver.Api.Features.ImportedPollingStation.Delete;

public class Endpoint : Endpoint<Request>
{
    private readonly IRepository<ImportedPollingStationEntity> _repository;

    public Endpoint(IRepository<ImportedPollingStationEntity> repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Delete("/import-job/{jobId:guid}/imported-polling-stations");
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        var importedPollingStation = await _repository.FirstOrDefaultAsync(new GetImportedPollingStationSpecification(request.JobId, request.Id), ct);
        if (importedPollingStation is not null)
        {
            await _repository.DeleteAsync(importedPollingStation, ct);
            await SendNoContentAsync(ct);
        }
        else
        {
            await SendNotFoundAsync(ct);
        }
    }
}
