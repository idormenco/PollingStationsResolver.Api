namespace PollingStationsResolver.Api.Features.PollingStation.List;

public record Request
{
    public int PageSize { get; init; }
    public int Page { get; init; }
}