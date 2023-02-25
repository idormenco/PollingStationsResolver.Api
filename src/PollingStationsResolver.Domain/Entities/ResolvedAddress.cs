using PollingStationsResolver.Domain.Helpers;

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

        Id = DeterministicGuid.Create(county, locality, address);
        Latitude = latitude;
        Longitude = longitude;
    }

    public string County { get; private set; }
    public string Locality { get; private set; }
    public string Address { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
}
