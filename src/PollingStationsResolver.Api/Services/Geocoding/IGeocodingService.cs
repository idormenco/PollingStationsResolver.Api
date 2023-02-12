namespace PollingStationsResolver.Api.Services.Geocoding;

public interface IGeocodingService
{
    Task<LocationSearchResult> FindCoordinatesAsync(string county, string fullAddress);
}