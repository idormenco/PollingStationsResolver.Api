using PollingStationsResolver.Api.Features.Common;

namespace PollingStationsResolver.Api.Features.PollingStation.Update;

public sealed record UpdatePollingStationRequest
{
    public Guid Id { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string County { get; init; }
    public string Locality { get; init; }
    public string PollingStationNumber { get; init; }
    public string Address { get; init; }

    public UpdateAssignedAddressRequest[] AssignedAddresses { get; init; }
}
