namespace PollingStationsResolver.Api.HangfireJobs;

public class HangfireJobActivator : Hangfire.JobActivator
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="HangfireJobActivator"/> class.
    /// </summary>
    /// <param name="serviceProvider">The app service provider.</param>
    public HangfireJobActivator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Activates the job.
    /// </summary>
    /// <param name="jobType">Type of the job.</param>
    /// <returns>object</returns>
    public override object ActivateJob(Type jobType)
    {
        return _serviceProvider.GetService(jobType)!;
    }
}
