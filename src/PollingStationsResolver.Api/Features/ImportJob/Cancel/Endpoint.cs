using PollingStationsResolver.Domain.Repository;

namespace PollingStationsResolver.Api.Features.ImportJob.Cancel;

public class Endpoint : EndpointWithoutRequest
{
    private readonly IRepository<Domain.Entities.ImportJobAggregate.ImportJob> _repository;

    public Endpoint(IRepository<Domain.Entities.ImportJobAggregate.ImportJob> repository)
    {
        _repository = repository;
    }
    public override void Configure()
    {
        Put("/import-job/cancel/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var jobId = Route<Guid>("id");
        var job = await _repository.GetByIdAsync(jobId, cancellationToken);

        if (job is not null)
        {
            job.Cancel();
            await _repository.UpdateAsync(job, cancellationToken);

            await SendNoContentAsync(cancellationToken);
        }
        else
        {
            await SendNotFoundAsync(cancellationToken);
        }
    }
}
