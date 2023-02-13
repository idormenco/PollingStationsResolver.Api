using PollingStationsResolver.Geocoding.Models;

namespace PollingStationsResolver.Geocoding.Interfaces;

internal interface IGeocodingClient
{
    Task<LocationSearchResult> FindCoordinatesAsync(string county, string address, CancellationToken cancellationToken);
}
