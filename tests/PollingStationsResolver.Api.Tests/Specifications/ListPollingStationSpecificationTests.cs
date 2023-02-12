using FluentAssertions;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Specifications;

namespace PollingStationsResolver.Api.Tests.Specifications;

public class ListPollingStationSpecificationTests
{
    [Fact]
    public void ListPollingStationSpecification_AppliesCorrectFilters()
    {

        var pollingStation1 = BobBuilder.CreatePollingStation();
        var pollingStation2 = BobBuilder.CreatePollingStation();

        var testCollection = Enumerable
            .Range(1, 10)
            .Select(_ => BobBuilder.CreatePollingStation())
            .Union(new[] { pollingStation1, pollingStation2 })
            .ToList();

        var pagination = new Pagination(2, 10);

        var spec = new ListPollingStationSpecification(pagination);
        var result = spec.Evaluate(testCollection).ToList();

        result.Should().HaveCount(2);
        result.Should().Contain(pollingStation1);
        result.Should().Contain(pollingStation2);
    }
}
