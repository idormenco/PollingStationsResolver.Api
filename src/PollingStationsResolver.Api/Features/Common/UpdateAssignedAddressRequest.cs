namespace PollingStationsResolver.Api.Features.Common;
public sealed record UpdateAssignedAddressRequest
{
    public Guid? Id { get; init; }
    public string Locality { get; init; }
    public string StreetCode { get; init; }
    public string Street { get; init; }
    public string HouseNumbers { get; init; }
    public string Remarks { get; init; }
}
