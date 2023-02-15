using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PollingStationsResolver.Domain.Repository;

namespace PollingStationsResolver.Domain;

public static class Installer
{
    public static IServiceCollection AddApplicationDomain(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContextPool<PollingStationsResolverContext>(options =>
            options.UseNpgsql(config.GetConnectionString("PollingStationsResolverApi"), sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null
                );
            }));

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));

        return services;
    }
}
