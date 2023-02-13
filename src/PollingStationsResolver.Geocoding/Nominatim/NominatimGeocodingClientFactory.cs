using Microsoft.Extensions.DependencyInjection;
using PollingStationsResolver.Geocoding.Interfaces;

namespace PollingStationsResolver.Geocoding.Nominatim;

internal class NominatimGeocodingClientFactory: IGeocodingClientFactory
{
    private readonly IServiceProvider _serviceProvider;

    public NominatimGeocodingClientFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IGeocodingClient Create()
    {
        return _serviceProvider.GetRequiredService<INominatimGeocodingClient>();
    }
}
