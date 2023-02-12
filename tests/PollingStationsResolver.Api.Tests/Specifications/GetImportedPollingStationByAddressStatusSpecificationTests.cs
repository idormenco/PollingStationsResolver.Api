using FluentAssertions;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;
using PollingStationsResolver.Domain.Specifications;

namespace PollingStationsResolver.Api.Tests.Specifications;

public class GetImportedPollingStationByAddressStatusSpecificationTests
{
    [Fact]
    public void GetImportedPollingStationByAddressStatusSpecification_AppliesCorrectFilters()
    {
        var requestedPollingStation1 = BobBuilder.CreateImportedPollingStation(status: ResolvedAddressStatus.Success);
        var requestedPollingStation2 = BobBuilder.CreateImportedPollingStation(status: ResolvedAddressStatus.Success);

        var testCollection = new List<ImportedPollingStation>()
        {
            requestedPollingStation1,
            requestedPollingStation2,
            BobBuilder.CreateImportedPollingStation(status: ResolvedAddressStatus.NotFound),
            BobBuilder.CreateImportedPollingStation(status: ResolvedAddressStatus.NotProcessed)
        };


        var spec = new GetImportedPollingStationByAddressStatusSpecification(ResolvedAddressStatus.Success);

        var result = spec.Evaluate(testCollection).ToList();

        result.Should().HaveCount(2);
        result.Should().Contain(requestedPollingStation1);
        result.Should().Contain(requestedPollingStation2);
    }
}
