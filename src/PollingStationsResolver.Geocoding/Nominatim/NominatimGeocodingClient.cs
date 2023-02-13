using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using PollingStationsResolver.Geocoding.Models;

namespace PollingStationsResolver.Geocoding.Nominatim;

internal class NominatimGeocodingClient : INominatimGeocodingClient
{
    private readonly HttpClient _httpClient;
    private ILogger<NominatimGeocodingClient> _logger;

    public NominatimGeocodingClient(HttpClient httpClient, ILogger<NominatimGeocodingClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<LocationSearchResult> FindCoordinatesAsync(string county, string fullAddress, CancellationToken cancellationToken)
    {

        try
        {
            // TODO: implement filtering by country
            using var response = await _httpClient.GetAsync($"/search?q={UrlEncoder.Default.Encode(county)}+{UrlEncoder.Default.Encode(fullAddress)}", cancellationToken);
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var searchResults = JsonSerializer.Deserialize<SearchResult[]>(responseString);

                if (searchResults!.Any())
                {
                    return new LocationSearchResult.Found(searchResults!.First().Lat, searchResults!.First().Lon);
                }
            }
            else
            {
                _logger.LogWarning(
                    "Received non success status code: {statusCode}, with response='{responseString}'",
                    response.StatusCode, responseString);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred when calling Nominatim service");
            return new LocationSearchResult.Error();
        }

        return new LocationSearchResult.NotFound();
    }
}
