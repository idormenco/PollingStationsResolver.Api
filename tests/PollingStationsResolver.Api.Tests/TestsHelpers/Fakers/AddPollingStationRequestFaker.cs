﻿using AutoBogus;
using PollingStationsResolver.Api.Features.PollingStation.Add;

namespace PollingStationsResolver.Api.Tests.TestsHelpers.Fakers;

public class AddPollingStationRequestFaker : AutoFaker<AddPollingStationRequest>
{
    public AddPollingStationRequestFaker()
    {
        RuleFor(fake => fake.PollingStationNumber, fake => fake.Address.BuildingNumber());
        RuleFor(fake => fake.County, fake => fake.Address.Country());
        RuleFor(fake => fake.Locality, fake => fake.Address.City());
        RuleFor(fake => fake.Address, fake => fake.Address.FullAddress());
        RuleFor(fake => fake.Latitude, fake => fake.Address.Latitude());
        RuleFor(fake => fake.Longitude, fake => fake.Address.Longitude());
        RuleFor(fake => fake.AssignedAddresses, () => new AddAssignedAddressRequestFaker().Generate(2).ToArray());
    }
}
