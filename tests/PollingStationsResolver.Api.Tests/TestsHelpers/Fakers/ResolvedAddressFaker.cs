using AutoBogus;
using PollingStationsResolver.Domain.Entities;

namespace PollingStationsResolver.Api.Tests.TestsHelpers.Fakers;

public class ResolvedAddressFaker : AutoFaker<ResolvedAddress>
{
    public ResolvedAddressFaker()
    {
        Ignore(x => x.Id);
        Ignore(x => x.Address);
        Ignore(x => x.County);
        Ignore(x => x.Locality);
        Ignore(x => x.Latitude);
        Ignore(x => x.Longitude);
        CustomInstantiator(faker => new ResolvedAddress(faker.Address.Country(), faker.Address.City(), faker.Address.FullAddress(), faker.Address.Latitude(), faker.Address.Longitude()));
    }
}
