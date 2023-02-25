using PollingStationsResolver.Geocoding.Models;

namespace PollingStationsResolver.Geocoding;

public interface IGetAddressCoordinatesQuery
{
    Task<LocationSearchResult> ExecuteAsync(string county, string locality, string address, CancellationToken cancellationToken = default);
}
