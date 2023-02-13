using System.Collections.Immutable;
using PollingStationsResolver.Geocoding.Interfaces;
using PollingStationsResolver.Geocoding.Models;
using Polly;

namespace PollingStationsResolver.Geocoding;

internal class GetAddressCoordinatesQuery : IGetAddressCoordinatesQuery
{
    private readonly CachePolicyFactory _cachePolicyFactory;
    private readonly ImmutableList<IGeocodingClientFactory> _geocodingClientFactories;

    public GetAddressCoordinatesQuery(IEnumerable<IGeocodingClientFactory> geocodingClientFactories, CachePolicyFactory cachePolicyFactory)
    {
        _cachePolicyFactory = cachePolicyFactory;
        _geocodingClientFactories = geocodingClientFactories.ToImmutableList();
    }

    public async Task<LocationSearchResult> ExecuteAsync(string county, string address, CancellationToken cancellationToken)
    {
        var cachePolicy = _cachePolicyFactory.GetCachePolicy();
        var cacheKey = BuildCacheKey(county, address);

        foreach (var geocodingClientFactory in _geocodingClientFactories)
        {
            var geocodingClient = geocodingClientFactory.Create();
            var result = await cachePolicy.ExecuteAsync(context => geocodingClient.FindCoordinatesAsync(county, address, cancellationToken), cacheKey);

            if (result is LocationSearchResult.Found)
            {
                return result;
            }
        }

        return new LocationSearchResult.NotFound();
    }

    private Context BuildCacheKey(string county, string address)
    {
        return new Context($"{county}/{address}");
    }
}
