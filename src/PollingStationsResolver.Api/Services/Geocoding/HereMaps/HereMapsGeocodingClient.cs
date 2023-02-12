using System.Text.Json;
using Microsoft.Extensions.Options;
using PollingStationsResolver.Api.Options;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Api.Services.Geocoding.HereMaps;

public class HereMapsGeocodingClient : IHereMapsGeocodingClient
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

    public async Task<LocationSearchResult> FindCoordinatesAsync(string county, string fullAddress)
    {
        try
        {
            using var response = await _client.GetAsync($"/v1/geocode?q={county} {fullAddress}&apiKey={_options.ApiKey}");
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var geocodeResponse = JsonSerializer.Deserialize<GeocodeResponse>(responseString);

                if (geocodeResponse!.Items.Any())
                {
                    return new LocationSearchResult
                    {
                        OperationStatus = ResolvedAddressStatus.Success,
                        Latitude = geocodeResponse.Items.First().Position.Lat,
                        Longitude = geocodeResponse.Items.First().Position.Lng
                    };
                }
            }
            else
            {
                _logger.LogWarning("Received non success status code: {statusCode}, with response='{responseString}'", response.StatusCode, responseString);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred when calling HereMaps service");
        }

        return new LocationSearchResult
        {
            OperationStatus = ResolvedAddressStatus.NotFound,
        };
    }
}