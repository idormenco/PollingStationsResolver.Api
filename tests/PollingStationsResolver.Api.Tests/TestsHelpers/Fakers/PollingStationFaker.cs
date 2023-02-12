using AutoBogus;
using PollingStationsResolver.Domain.Entities.PollingStationAggregate;

namespace PollingStationsResolver.Api.Tests.TestsHelpers.Fakers;

public class PollingStationFaker : AutoFaker<PollingStation>
{
    public PollingStationFaker(Guid id) : base(binder: new PrivatePropertyBinder())
    {
        RuleFor(fake => fake.Id, () => id);
        RuleFor(fake => fake.PollingStationNumber, fake => fake.Address.BuildingNumber());
        RuleFor(fake => fake.County, fake => fake.Address.Country());
        RuleFor(fake => fake.Locality, fake => fake.Address.City());
        RuleFor(fake => fake.Address, fake => fake.Address.FullAddress());
        RuleFor(fake => fake.Latitude, fake => fake.Address.Latitude());
        RuleFor(fake => fake.Longitude, fake => fake.Address.Longitude());
        RuleFor("_assignedAddresses", (_, _) => new AssignedAddressFaker().Generate(2));
    }
}
