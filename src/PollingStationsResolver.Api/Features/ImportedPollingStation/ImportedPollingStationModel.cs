using System.Collections.Immutable;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Api.Features.ImportedPollingStation;

public sealed record ImportedPollingStationModel
{
    public Guid Id { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public string County { get; init; }
    public string Locality { get; init; }
    public string PollingStationNumber { get; init; }
    public string Address { get; init; }
    public Guid JobId { get; init; }
    public ResolvedAddressStatus ResolvedAddressStatus { get; init; }

    public IImmutableList<AddressModel> AssignedAddresses { get; init; }

    public class AddressModel
    {
        public Guid Id { get; init; }
        public string? StreetCode { get; init; }
        public string? Street { get; init; }
        public string? HouseNumbers { get; init; }
        public string? Remarks { get; init; }
        public string? Locality { get; init; }
    }
}
