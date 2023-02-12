using System.Collections.Immutable;

namespace PollingStationsResolver.Api.Features.PollingStation;

public class PollingStationModel
{
    public Guid Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string County { get; set; }
    public string Locality { get; set; }
    public string PollingStationNumber { get; set; }
    public string Address { get; set; }
    public IImmutableList<AddressModel> AssignedAddresses { get; set; }
    public string Institution { get; set; }

    public class AddressModel
    {
        public Guid Id { get; set; }
        public string? StreetCode { get; set; }
        public string? Street { get; set; }
        public string? HouseNumbers { get; set; }
        public string? Remarks { get; set; }
        public string? Locality { get; set; }
    }
}
