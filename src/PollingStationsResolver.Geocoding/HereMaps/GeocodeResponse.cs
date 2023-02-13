namespace PollingStationsResolver.Geocoding.HereMaps;

internal class GeocodeResponse
{
    public Item[] Items { get; init; }

    public class Item
    {
        public string Title { get; init; }
        public string Id { get; init; }
        public string ResultType { get; init; }
        public string LocalityType { get; init; }
        public Coordinates Position { get; init; }

        public class Coordinates
        {
            public float Lat { get; init; }
            public float Lng { get; init; }
        }
    }
}
