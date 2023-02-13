using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;

namespace PollingStationsResolver.Api.Features.ImportJob.GetCurrentInProgress;

public class Endpoint : EndpointWithoutRequest<ImportJobModel, ResponseMapper>
{
    private readonly IRepository<Domain.Entities.ImportJobAggregate.ImportJob> _repository;

    public Endpoint(IRepository<Domain.Entities.ImportJobAggregate.ImportJob> repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Get("/import-job/current-in-progress");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var entity = await _repository.FirstOrDefaultAsync(new CurrentImportJobInProgressSpecification(), ct);
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
