using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PollingStationsResolver.Geocoding.Models;
using PollingStationsResolver.Geocoding.Options;

namespace PollingStationsResolver.Geocoding.HereMaps;

internal class HereMapsGeocodingClient : IHereMapsGeocodingClient
{
    private readonly ILogger<HereMapsGeocodingClient> _logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly HereMapsOptions _options;
    private readonly HttpClient _client;
    public HereMapsGeocodingClient(HttpClient client,
        ILogger<HereMapsGeocodingClient> logger,
        JsonSerializerOptions jsonSerializerOptions,
        IOptions<HereMapsOptions> options)
    {
        _client = client;
        _logger = logger;
        _jsonSerializerOptions = jsonSerializerOptions;
        _options = options.Value;
    }

    public async Task<LocationSearchResult> FindCoordinatesAsync(string county, string locality, string address, CancellationToken cancellationToken)
    {
        try
        {
            using var response = await _client.GetAsync($"/geocode?q={county}_{address}&apiKey={_options.ApiKey}", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var geocodeResponse = JsonSerializer.Deserialize<GeocodeResponse>(contentStream, _jsonSerializerOptions);

                if (geocodeResponse!.Items.Any())
                {
                    var coordinates = geocodeResponse.Items.First().Position;
                    return new LocationSearchResult.Found(coordinates.Lat, coordinates.Lng);
                }
            }
            else
            {
                var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogWarning("[HereMaps] Received non success status code: {statusCode}, with response='{responseString}' for {county}, {locality}, {address}", response.StatusCode, responseString, county, locality, address);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred when calling HereMaps service for {county}, {locality}, {address}", county, locality, address);
            return new LocationSearchResult.Error();
        }

        return new LocationSearchResult.NotFound();
    }
}
