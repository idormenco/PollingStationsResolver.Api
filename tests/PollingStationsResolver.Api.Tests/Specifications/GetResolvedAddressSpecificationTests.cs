using FluentAssertions;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities;
using PollingStationsResolver.Domain.Specifications;

namespace PollingStationsResolver.Api.Tests.Specifications;

public class GetResolvedAddressSpecificationTests
{
    [Fact]
    public void GetResolvedAddressSpecification_AppliesCorrectFilters()
    {
        var resolvedAddress = BobBuilder.CreateResolvedAddress();

        var testCollection = new List<ResolvedAddress>()
        {
            resolvedAddress,
            BobBuilder.CreateResolvedAddress(),
            BobBuilder.CreateResolvedAddress(),
            BobBuilder.CreateResolvedAddress()
        };


        var spec = new GetResolvedAddressSpecification(resolvedAddress.County, resolvedAddress.Locality, resolvedAddress.Address);

        var result = spec.Evaluate(testCollection).FirstOrDefault();

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(resolvedAddress);
    }
}
