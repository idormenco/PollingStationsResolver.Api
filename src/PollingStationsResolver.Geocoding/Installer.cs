using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PollingStationsResolver.Geocoding.HereMaps;
using PollingStationsResolver.Geocoding.Interfaces;
using PollingStationsResolver.Geocoding.Nominatim;
using PollingStationsResolver.Geocoding.Options;
using Polly;
using Polly.Caching;
using Polly.Caching.Memory;

namespace PollingStationsResolver.Geocoding;

public static class Installer
{
    public static IServiceCollection AddLocationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddMemoryCache();
        services.AddSingleton<IAsyncCacheProvider, MemoryCacheProvider>();
        services.AddSingleton<CachePolicyFactory>();

        var hereMapsSection = config.GetSection($"Geocoding:{HereMapsOptions.SectionKey}");
        var nominatimSection = config.GetSection($"Geocoding:{NominatimOptions.SectionKey}");

        services.Configure<HereMapsOptions>(hereMapsSection);

        services.AddHttpClient<INominatimGeocodingClient, NominatimGeocodingClient>(httpClient =>
        {
            var nominatimOptions = nominatimSection.Get<NominatimOptions>()!;
            httpClient.BaseAddress = new Uri(nominatimOptions.BaseUrl);
        }).AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)));

        services.AddHttpClient<IHereMapsGeocodingClient, HereMapsGeocodingClient>(httpClient =>
        {
            var hereOptions = hereMapsSection.Get<HereMapsOptions>()!;
            httpClient.BaseAddress = new Uri(hereOptions.BaseUrl);
        }).AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)));

        services.AddSingleton<IGeocodingClientFactory, NominatimGeocodingClientFactory>();
        services.AddSingleton<IGeocodingClientFactory, HereMapsGeocodingClientFactory>();
        services.AddSingleton<IGetAddressCoordinatesQuery, GetAddressCoordinatesQuery>();

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
        };

        services.AddSingleton(jsonSerializerOptions);
        return services;
    }
}
