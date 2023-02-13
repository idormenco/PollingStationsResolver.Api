using PollingStationsResolver.Geocoding.Models;
using Polly;
using Polly.Caching;

namespace PollingStationsResolver.Geocoding;

internal class CachePolicyFactory
{
    private readonly IAsyncCacheProvider _cacheProvider;
    private IAsyncPolicy<LocationSearchResult>? _cachePolicy;

    public CachePolicyFactory(IAsyncCacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider;
    }

    public IAsyncPolicy<LocationSearchResult> GetCachePolicy()
    {
        if (_cachePolicy is null)
        {
            Func<Context, LocationSearchResult, Ttl> cacheOnlyResolvedFilter =
                (context, result) => new Ttl(
                    timeSpan: result is LocationSearchResult.Found ? TimeSpan.FromSeconds(15) : TimeSpan.Zero,
                    slidingExpiration: true
                );

            _cachePolicy =
                Policy.CacheAsync(
                    _cacheProvider.AsyncFor<LocationSearchResult>(),
                    new ResultTtl<LocationSearchResult>(cacheOnlyResolvedFilter),
                    onCacheError: null);
        }

        return _cachePolicy;
    }
}
