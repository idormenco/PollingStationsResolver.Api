using PollingStationsResolver.Domain.Repository;

namespace PollingStationsResolver.Api.Features.ImportJob.Get;

public class Endpoint : EndpointWithoutRequest<ImportJobModel, ResponseMapper>
{
    private readonly IRepository<Domain.Entities.ImportJobAggregate.ImportJob> _repository;

    public Endpoint(IRepository<Domain.Entities.ImportJobAggregate.ImportJob> repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Get("/import-job/{id:guid}");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var jobId = Route<Guid>("id");

        var entity = await _repository.GetByIdAsync(jobId, ct);
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
