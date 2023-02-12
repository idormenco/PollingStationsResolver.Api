namespace PollingStationsResolver.Domain.Entities.PollingStationAggregate;

public class AssignedAddress : BaseEntity
{
#pragma warning disable CS8618 // Required by Entity Framework
    private AssignedAddress()
    {
    }

    public AssignedAddress(string locality, string streetCode, string street, string houseNumbers, string remarks)
    {
        Locality = locality;
        StreetCode = streetCode;
        Street = street;
        HouseNumbers = houseNumbers;
        Remarks = remarks;
    }

    public string Locality { get; private set; }
    public string StreetCode { get; private set; }
    public string Street { get; private set; }
    public string HouseNumbers { get; private set; }
    public string Remarks { get; private set; }

    public void UpdateDetails(string locality, string streetCode, string street, string houseNumbers, string remarks)
    {
        Locality = locality;
        StreetCode = streetCode;
        Street = street;
        HouseNumbers = houseNumbers;
        Remarks = remarks;
    }
}