using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;

namespace PollingStationsResolver.Api.Features.ImportJob.GetFile;

public class Endpoint : EndpointWithoutRequest
{
    private readonly IRepository<Domain.Entities.ImportJobAggregate.ImportJob> _repository;

    public Endpoint(IRepository<Domain.Entities.ImportJobAggregate.ImportJob> repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Get("/import-job/{id:guid}/file");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var jobId = Route<Guid>("id");

        var entity = await _repository.FirstOrDefaultAsync(new GetImportJobSpecification(jobId, true), ct);
        if (entity is not null)
        {
            await SendBytesAsync(Convert.FromBase64String(entity.File.Base64File), fileName: entity.FileName, cancellation: ct);
        }
        else
        {
            await SendNotFoundAsync(ct);
        }
    }
}
