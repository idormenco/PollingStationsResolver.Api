using Microsoft.Extensions.DependencyInjection;
using PollingStationsResolver.Geocoding.Interfaces;

namespace PollingStationsResolver.Geocoding.HereMaps;

internal class HereMapsGeocodingClientFactory : IGeocodingClientFactory
{
    private readonly IServiceProvider _serviceProvider;

    public HereMapsGeocodingClientFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IGeocodingClient Create()
    {
        return _serviceProvider.GetRequiredService<IHereMapsGeocodingClient>();
    }
}
