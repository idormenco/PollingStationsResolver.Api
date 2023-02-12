namespace PollingStationsResolver.Api.Jobs;

public interface IGeocodingJob
{
    Task Run(Guid jobId, Guid pollingStationId, CancellationToken cancellationToken);
}