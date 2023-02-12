namespace PollingStationsResolver.Api.Jobs;

public interface IImportJobStatusUpdaterJob
{
    Task Run(CancellationToken cancellationToken);
}