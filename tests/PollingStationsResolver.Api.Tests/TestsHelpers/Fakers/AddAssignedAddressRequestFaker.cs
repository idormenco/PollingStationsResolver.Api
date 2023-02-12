using AutoBogus;
using PollingStationsResolver.Api.Features.Common;

namespace PollingStationsResolver.Api.Tests.TestsHelpers.Fakers;

public class AddAssignedAddressRequestFaker : AutoFaker<AddAssignedAddressRequest>
{
    public AddAssignedAddressRequestFaker()
    {
        RuleFor(fake => fake.Locality, fake => fake.Address.City());
        RuleFor(fake => fake.StreetCode, fake => fake.Address.ZipCode());
        RuleFor(fake => fake.Street, fake => fake.Address.StreetName());
        RuleFor(fake => fake.HouseNumbers, fake => fake.Address.BuildingNumber());
        RuleFor(fake => fake.Remarks, fake => fake.Lorem.Sentence());
    }
}
