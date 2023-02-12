using AutoBogus;
using PollingStationsResolver.Api.Features.ImportedPollingStation.Update;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Api.Tests.TestsHelpers.Fakers;

public class UpdateImportedPollingStationRequestFaker : AutoFaker<UpdateImportedPollingStationRequest>
{
    public UpdateImportedPollingStationRequestFaker(Guid? id)
    {
        RuleFor(fake => fake.Id, () => id ?? Guid.NewGuid());
        RuleFor(fake => fake.JobId, fake => fake.Random.Guid());
        RuleFor(fake => fake.PollingStationNumber, fake => fake.Address.BuildingNumber());
        RuleFor(fake => fake.County, fake => fake.Address.Country());
        RuleFor(fake => fake.Locality, fake => fake.Address.City());
        RuleFor(fake => fake.Address, fake => fake.Address.FullAddress());
        RuleFor(fake => fake.Latitude, fake => fake.Address.Latitude());
        RuleFor(fake => fake.Longitude, fake => fake.Address.Longitude());
        RuleFor(fake => fake.ResolvedAddressStatus, fake => fake.PickRandom(ResolvedAddressStatus.Success, ResolvedAddressStatus.NotFound, ResolvedAddressStatus.NotProcessed));

    }
}
