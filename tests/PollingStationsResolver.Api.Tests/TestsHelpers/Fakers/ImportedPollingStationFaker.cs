using AutoBogus;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Api.Tests.TestsHelpers.Fakers;

public class ImportedPollingStationFaker : AutoFaker<ImportedPollingStation>
{
    public ImportedPollingStationFaker(Guid jobId, Guid id, ResolvedAddressStatus? status = null) : base(binder: new PrivatePropertyBinder())
    {
        RuleFor(fake => fake.Id, () => id);
        RuleFor(fake => fake.JobId, () => jobId);
        RuleFor(fake => fake.PollingStationNumber, fake => fake.Address.BuildingNumber());
        RuleFor(fake => fake.County, fake => fake.Address.Country());
        RuleFor(fake => fake.Locality, fake => fake.Address.City());
        RuleFor(fake => fake.Address, fake => fake.Address.FullAddress());
        RuleFor(fake => fake.Latitude, fake => fake.Address.Latitude());
        RuleFor(fake => fake.Longitude, fake => fake.Address.Longitude());
        RuleFor(fake => fake.ResolvedAddressStatus, fake => status ?? fake.PickRandom(ResolvedAddressStatus.Success, ResolvedAddressStatus.NotFound, ResolvedAddressStatus.NotProcessed));
        RuleFor("_assignedAddresses", (_, _) => new ImportedPollingStationAddressFaker().Generate(3));
    }
}
