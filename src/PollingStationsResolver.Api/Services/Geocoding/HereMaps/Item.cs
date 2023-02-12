namespace PollingStationsResolver.Api.Services.Geocoding.HereMaps;

public class Item
{
    public string Title { get; init; }
    public string Id { get; init; }
    public string ResultType { get; init; }
    public string LocalityType { get; init; }
    public Position Position { get; init; }
}