namespace PollingStationsResolver.Api.HangfireJobs;

public interface IImportJobStatusUpdaterJob
{
    Task Run(CancellationToken cancellationToken);
}
