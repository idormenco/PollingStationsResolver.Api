using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Api.Services.Geocoding;

public record LocationSearchResult
{
    public ResolvedAddressStatus OperationStatus { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
}