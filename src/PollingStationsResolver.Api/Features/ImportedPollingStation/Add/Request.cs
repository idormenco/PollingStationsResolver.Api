using System.Text.Json.Serialization;
using Ardalis.SmartEnum.JsonNet;
using PollingStationsResolver.Api.Features.Common;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Api.Features.ImportedPollingStation.Add;

public record AddImportedPollingStationRequest
{
    public Guid JobId { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string County { get; init; }
    public string Locality { get; init; }
    public string PollingStationNumber { get; init; }
    public string Address { get; init; }

    [JsonConverter(typeof(SmartEnumNameConverter<ResolvedAddressStatus, int>))]
    public ResolvedAddressStatus ResolvedAddressStatus { get; init; }

    public AddAssignedAddressRequest[] AssignedAddresses { get; init; }
}
