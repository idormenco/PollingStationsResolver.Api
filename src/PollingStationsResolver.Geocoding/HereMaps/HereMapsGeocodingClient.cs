using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PollingStationsResolver.Geocoding.Models;
using PollingStationsResolver.Geocoding.Options;

namespace PollingStationsResolver.Geocoding.HereMaps;

internal class HereMapsGeocodingClient : IHereMapsGeocodingClient
{
    private readonly ILogger<HereMapsGeocodingClient> _logger;
    private readonly HereMapsOptions _options;
    private readonly HttpClient _client;
    public HereMapsGeocodingClient(HttpClient client, ILogger<HereMapsGeocodingClient> logger, IOptions<HereMapsOptions> options)
    {
        _client = client;
        _logger = logger;
        _options = options.Value;
    }

    public async Task<LocationSearchResult> FindCoordinatesAsync(string county, string address, CancellationToken cancellationToken)
    {
        try
        {
            using var response = await _client.GetAsync($"/v1/geocode?q={county} {address}&apiKey={_options.ApiKey}", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var geocodeResponse = JsonSerializer.Deserialize<GeocodeResponse>(contentStream);

                if (geocodeResponse!.Items.Any())
                {
                    var coordinates = geocodeResponse.Items.First().Position;
                    return new LocationSearchResult.Found(coordinates.Lat, coordinates.Lng);
                }
            }
            else
            {
                var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogWarning("Received non success status code: {statusCode}, with response='{responseString}'", response.StatusCode, responseString);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred when calling HereMaps service");
            return new LocationSearchResult.Error();
        }

        return new LocationSearchResult.NotFound();
    }
}
