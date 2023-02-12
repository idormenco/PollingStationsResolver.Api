using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Api.Features.ImportJob;

public sealed record ImportJobModel
{
    public Guid Id { get; init; }
    public ImportJobStatus JobStatus { get; init; }
    public DateTime? StartedAt { get; init; }
    public DateTime? FinishedAt { get; init; }
    public string FileName { get; init; }
    public Guid FileId { get; set; }
}
