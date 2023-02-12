namespace PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;

public class ImportedPollingStationAddress : BaseEntity
{
#pragma warning disable CS8618 // Required by Entity Framework
    public ImportedPollingStationAddress()
    {

    }

    public ImportedPollingStationAddress(string locality, string street, string streetCode, string houseNumbers, string remarks)
    {
        Locality = locality;
        Street = street;
        StreetCode = streetCode;
        HouseNumbers = houseNumbers;
        Remarks = remarks;
    }

    public string Locality { get; private set; }
    public string? Street { get; private set; }
    public string? StreetCode { get; private set; }
    public string? HouseNumbers { get; private set; }
    public string? Remarks { get; private set; }

    public void UpdateDetails(string locality, string streetCode, string street, string houseNumbers, string remarks)
    {
        Locality = locality;
        StreetCode = streetCode;
        Street = street;
        HouseNumbers = houseNumbers;
        Remarks = remarks;
    }
}
