using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using PollingStationsResolver.Geocoding.Models;

namespace PollingStationsResolver.Geocoding.Nominatim;

internal class NominatimGeocodingClient : INominatimGeocodingClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly ILogger<NominatimGeocodingClient> _logger;

    public NominatimGeocodingClient(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions, ILogger<NominatimGeocodingClient> logger)
    {
        _httpClient = httpClient;
        _jsonSerializerOptions = jsonSerializerOptions;
        _logger = logger;
    }

    public async Task<LocationSearchResult> FindCoordinatesAsync(string county, string locality, string address, CancellationToken cancellationToken)
    {
        try
        {
            using var response = await _httpClient.GetAsync($"/search?q={UrlEncoder.Default.Encode(county)}+{UrlEncoder.Default.Encode(address)}&format=json", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var searchResults = JsonSerializer.Deserialize<SearchResult[]>(contentStream, _jsonSerializerOptions);

                if (searchResults!.Any())
                {
                    return new LocationSearchResult.Found(searchResults!.First().Lat, searchResults!.First().Lon);
                }
            }
            else
            {
                var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogWarning("[Nominatim] Received non success status code: {statusCode}, with response='{responseString}' for {county}, {locality}, {address}", response.StatusCode, responseString, county, locality, address);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred when calling Nominatim service for {county}, {locality}, {address}", county, locality, address);
            return new LocationSearchResult.Error();
        }

        _logger.LogWarning("[Nominatim] No coordinates found for {county}, {locality}, {address}", county, locality, address);

        return new LocationSearchResult.NotFound();
    }
}
