using AutoBogus;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;

namespace PollingStationsResolver.Api.Tests.TestsHelpers.Fakers;

public class ImportedPollingStationAddressFaker : AutoFaker<ImportedPollingStationAddress>
{
    public ImportedPollingStationAddressFaker()
    {
        RuleFor(fake => fake.Id, fake => fake.Random.Guid());
        RuleFor(fake => fake.Locality, fake => fake.Address.City());
        RuleFor(fake => fake.StreetCode, fake => fake.Address.ZipCode());
        RuleFor(fake => fake.Street, fake => fake.Address.StreetName());
        RuleFor(fake => fake.HouseNumbers, fake => fake.Address.BuildingNumber());
        RuleFor(fake => fake.Remarks, fake => fake.Lorem.Sentence());
    }
}
