namespace PollingStationsResolver.Api.HangfireJobs;

public interface IGeocodingJob
{
    Task Run(Guid jobId, Guid pollingStationId, CancellationToken cancellationToken);
}
