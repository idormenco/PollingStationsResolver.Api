namespace PollingStationsResolver.Api.Features.ImportJob.Create;

public sealed record ImportRequest
{
    public IFormFile File { get; init; }
}