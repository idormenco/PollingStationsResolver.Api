using PollingStationsResolver.Api.Features.Common;

namespace PollingStationsResolver.Api.Features.PollingStation.Add;

public record AddPollingStationRequest
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string County { get; init; }
    public string Locality { get; init; }
    public string PollingStationNumber { get; init; }
    public string Address { get; init; }

    public AddAssignedAddressRequest[] AssignedAddresses { get; init; }

}
