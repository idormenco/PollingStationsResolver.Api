namespace PollingStationsResolver.Api.Features.ImportedPollingStation.List;

public record Request
{
    public Guid JobId { get; init; }
    public int PageSize { get; init; }
    public int Page { get; init; }
}