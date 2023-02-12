namespace PollingStationsResolver.Domain.Entities;

public class ResolvedAddress : BaseEntity, IAggregateRoot
{
#pragma warning disable CS8618 // Required by Entity Framework
    public ResolvedAddress()
    {

    }
    public ResolvedAddress(string county, string locality, string address, double latitude, double longitude)
    {
        County = county;
        Locality = locality;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
    }

    public string County { get; private init; }
    public string Locality { get; private init; }
    public string Address { get; private init; }
    public double Latitude { get; private init; }
    public double Longitude { get; private init; }
}