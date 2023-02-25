using Ardalis.GuardClauses;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;

public class ImportedPollingStation : BaseEntity, IAggregateRoot
{
#pragma warning disable CS8618 // Required by Entity Framework
    public ImportedPollingStation()
    {
    }

    public ImportedPollingStation(string pollingStationNumber, string county, string locality, string address, double? latitude, double? longitude, ResolvedAddressStatus resolvedAddressStatus)
    {
        PollingStationNumber = pollingStationNumber;
        County = county;
        Locality = locality;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
        ResolvedAddressStatus = resolvedAddressStatus;
    }

    public double? Latitude { get; private set; }
    public double? Longitude { get; private set; }
    public string County { get; private set; }
    public string Locality { get; private set; }
    public string PollingStationNumber { get; private set; }
    public string Address { get; private set; }
    public Guid JobId { get; private set; }
    public ResolvedAddressStatus ResolvedAddressStatus { get; private set; }

    // DDD Patterns comment
    // Using a private collection field, better for DDD Aggregate's encapsulation
    // so AssignedAddresses cannot be added from "outside the AggregateRoot" directly to the collection,
    // but only through the method PollingStation.AddAssignedAddress() which includes behavior.
    private readonly List<ImportedPollingStationAddress> _assignedAddresses = new();

    // Using List<>.AsReadOnly() 
    // This will create a read only wrapper around the private list so is protected against "external updates".
    // It's much cheaper than .ToList() because it will not have to copy all items in a new collection. (Just one heap alloc for the wrapper instance)
    //https://msdn.microsoft.com/en-us/library/e78dcd75(v=vs.110).aspx 
    public IReadOnlyCollection<ImportedPollingStationAddress> AssignedAddresses => _assignedAddresses.AsReadOnly();

    public void UpdateDetails(string pollingStationNumber, string county, string locality, string address, double? latitude, double? longitude, ResolvedAddressStatus resolvedAddressStatus)
    {
        PollingStationNumber = pollingStationNumber;
        County = county;
        Locality = locality;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
        ResolvedAddressStatus = resolvedAddressStatus;
    }

    public void AddAssignedAddress(string locality, string streetCode, string street, string houseNumbers, string remarks)
    {
        var assignedAddress = new ImportedPollingStationAddress(locality, street, streetCode, houseNumbers, remarks);
        _assignedAddresses.Add(assignedAddress);
    }

    public void UpdateAddress(Guid id, string locality, string streetCode, string street, string houseNumbers, string remarks)
    {
        var address = GetAddressById(id);
        Guard.Against.Null(address, message: "Cannot find address with requested id.", parameterName: nameof(id));
        address.UpdateDetails(locality, streetCode, street, houseNumbers, remarks);
    }

    public void DeleteAddress(Guid id)
    {
        var address = GetAddressById(id);
        Guard.Against.Null(address, message: "Cannot find address with requested id.", parameterName: nameof(id));
        _assignedAddresses.Remove(address);
    }

    public void AssignToJob(Guid jobId)
    {
        Guard.Against.NullOrEmpty(jobId);
        JobId = jobId;
    }
    private ImportedPollingStationAddress? GetAddressById(Guid id)
    {
        return _assignedAddresses.Find(address => address.Id == id);
    }

    public void UpdateCoordinates(double latitude, double longitude)
    {
        ResolvedAddressStatus = ResolvedAddressStatus.Success;
        Latitude = latitude;
        Longitude = longitude;
    }

    public void MarkAsNotFound()
    {
        ResolvedAddressStatus = ResolvedAddressStatus.NotFound;
    }
}
