using PollingStationsResolver.Api.Options;
using PollingStationsResolver.Api.Services.Geocoding.HereMaps;
using PollingStationsResolver.Api.Services.Geocoding.Nominatim;

namespace PollingStationsResolver.Api.Services.Geocoding;

public static class Installer
{
    public static IServiceCollection AddLocationServices(this IServiceCollection services, IConfiguration config)
    {
        var hereMapsSection = config.GetSection($"Geocoding:{HereMapsOptions.SectionKey}");
        var nominatimSection = config.GetSection($"Geocoding:{NominatimOptions.SectionKey}");

        services.Configure<HereMapsOptions>(hereMapsSection);

        services.AddHttpClient<IGeocodingService, NominatimGeocodingClient>(httpClient =>
        {
            var nominatimOptions = nominatimSection.Get<NominatimOptions>()!;
            httpClient.BaseAddress = new Uri(nominatimOptions.BaseUrl);
        });

        services.AddHttpClient<IGeocodingService, HereMapsGeocodingClient>(httpClient =>
        {
            var hereOptions = hereMapsSection.Get<HereMapsOptions>()!;
            httpClient.BaseAddress = new Uri(hereOptions.BaseUrl);
        });

        return services;
    }
}