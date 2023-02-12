using System.Text.Encodings.Web;
using System.Text.Json;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Api.Services.Geocoding.Nominatim;

public class NominatimGeocodingClient : INominatimGeocodingClient
{
    private readonly HttpClient _httpClient;
    private ILogger<NominatimGeocodingClient> _logger;

    public NominatimGeocodingClient(HttpClient httpClient, ILogger<NominatimGeocodingClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<LocationSearchResult> FindCoordinatesAsync(string county, string fullAddress)
    {

        try
        {
            // TODO: implement filtering by country
            using var response = await _httpClient.GetAsync($"/search?q={UrlEncoder.Default.Encode(county)}+{UrlEncoder.Default.Encode(fullAddress)}");
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var searchResults = JsonSerializer.Deserialize<SearchResult[]>(responseString);

                if (searchResults!.Any())
                {
                    return new LocationSearchResult
                    {
                        OperationStatus = ResolvedAddressStatus.Success,
                        Latitude = searchResults!.First().Lat,
                        Longitude = searchResults!.First().Lon
                    };
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
        }

        return new LocationSearchResult
        {
            OperationStatus = ResolvedAddressStatus.NotFound,
        };
    }
}