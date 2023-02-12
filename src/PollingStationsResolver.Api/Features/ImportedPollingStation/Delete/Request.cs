namespace PollingStationsResolver.Api.Features.ImportedPollingStation.Delete;

public record Request
{
    public Guid JobId { get; init; }
    public Guid Id { get; init; }

}
