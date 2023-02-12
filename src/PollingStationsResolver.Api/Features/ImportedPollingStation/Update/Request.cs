using PollingStationsResolver.Api.Features.Common;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Api.Features.ImportedPollingStation.Update;

public record UpdateImportedPollingStationRequest
{
    public Guid JobId { get; init; }
    public Guid Id { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string County { get; init; }
    public string Locality { get; init; }
    public string PollingStationNumber { get; init; }
    public string Address { get; init; }
    public ResolvedAddressStatus ResolvedAddressStatus { get; init; }

    public UpdateAssignedAddressRequest[] AssignedAddresses { get; init; }
}
