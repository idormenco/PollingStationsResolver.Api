using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.MemoryStorage;

namespace PollingStationsResolver.Api.HangfireJobs;

public static class Installer
{
    public static IServiceCollection AddHangfireJobs(this IServiceCollection services)
    {
        // Add Hangfire services.
        services.AddTransient<IGeocodingJob, GeocodingJob>();

        services.AddHangfire(c => c
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseMemoryStorage()
        );

        // Add the processing server as IHostedService
        services.AddHangfireServer();

        return services;
    }

    public static IApplicationBuilder UseHangfireJobs(this IApplicationBuilder app, IConfiguration configuration)
    {
        GlobalConfiguration.Configuration.UseActivator(new HangfireJobActivator(app.ApplicationServices));

        app.UseHangfireDashboard(options: new DashboardOptions
        {
            DashboardTitle = "PollingStationResolver API Dashboard",
            Authorization = new IDashboardAuthorizationFilter[]
            {
                new BasicAuthAuthorizationFilter(
                    new BasicAuthAuthorizationFilterOptions
                    {
                        // Require secure connection for dashboard
                        RequireSsl = false,
                        SslRedirect = false,
                        // Case sensitive login checking
                        LoginCaseSensitive = true,
                        // Users
                        Users = new[]
                        {
                            new BasicAuthAuthorizationUser
                            {
                                Login = configuration["AdminCredentials:Username"],
                                // Password as plain text, SHA1 will be used
                                PasswordClear = configuration["AdminCredentials:Password"]
                            }
                        }
                    })
            }
        });

        RecurringJob.AddOrUpdate<ImportJobStatusUpdaterJob>(x => x.Run(CancellationToken.None), Cron.Minutely(), timeZone: TimeZoneInfo.Utc);

        return app;
    }
}
