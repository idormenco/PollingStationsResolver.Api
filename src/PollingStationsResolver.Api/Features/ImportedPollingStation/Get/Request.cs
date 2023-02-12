namespace PollingStationsResolver.Api.Features.ImportedPollingStation.Get;

public record Request
{
    public Guid JobId { get; init; }
    public Guid Id { get; init; }

}