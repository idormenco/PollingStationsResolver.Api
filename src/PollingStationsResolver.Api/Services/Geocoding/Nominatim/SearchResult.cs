using System.Text.Json.Serialization;

namespace PollingStationsResolver.Api.Services.Geocoding.Nominatim;

public class SearchResult
{
    [JsonPropertyName("place_id")]
    public int PlaceId { get; set; }

    [JsonPropertyName("licence")]
    public string Licence { get; set; }

    [JsonPropertyName("osm_type")]
    public string OsmType { get; set; }

    [JsonPropertyName("osm_id")]
    public int OsmId { get; set; }

    [JsonPropertyName("boundingbox")]
    public List<string> Boundingbox { get; set; }

    [JsonPropertyName("lat")]
    public double Lat { get; set; }

    [JsonPropertyName("lon")]
    public double Lon { get; set; }
    
    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; }

    [JsonPropertyName("place_rank")]
    public int PlaceRank { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("importance")]
    public double Importance { get; set; }
}