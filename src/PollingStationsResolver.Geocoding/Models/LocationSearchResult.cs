namespace PollingStationsResolver.Geocoding.Models;

public abstract record LocationSearchResult
{
    public record Error : LocationSearchResult;
    public record Found(double Latitude, double Longitude) : LocationSearchResult;
    public record NotFound : LocationSearchResult;
    private LocationSearchResult() { }
}
