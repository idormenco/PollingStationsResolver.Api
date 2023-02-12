using FluentAssertions;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.PollingStationAggregate;
using PollingStationsResolver.Domain.Specifications;

namespace PollingStationsResolver.Api.Tests.Specifications;

public class GetPollingStationSpecificationTests
{
    [Fact]
    public void GetPollingStationSpecification_AppliesCorrectFilters()
    {
        var requestedPollingStation = BobBuilder.CreatePollingStation();
        var testCollection = new List<PollingStation>()
        {
            requestedPollingStation,
            BobBuilder.CreatePollingStation(),
            BobBuilder.CreatePollingStation()
        };


        var spec = new GetPollingStationSpecification(requestedPollingStation.Id);

        var result = spec.Evaluate(testCollection).FirstOrDefault();

        result.Should().BeEquivalentTo(requestedPollingStation);
    }
}
