using FluentAssertions;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Specifications;

namespace PollingStationsResolver.Api.Tests.Specifications;

public class ListImportedPollingStationsSpecificationTests
{
    [Fact]
    public void ListImportedPollingStationsSpecification_AppliesCorrectFilters()
    {
        var jobId1 = Guid.NewGuid();
        var jobId2 = Guid.NewGuid();

        var pollingStation1 = BobBuilder.CreateImportedPollingStation(jobId1);
        var pollingStation2 = BobBuilder.CreateImportedPollingStation(jobId1);

        var testCollection = Enumerable
              .Range(1, 10)
              .Select(_ => BobBuilder.CreateImportedPollingStation(jobId1))
              .Union(new[] { pollingStation1, pollingStation2 })
              .Union(Enumerable.Range(1, 100).Select(_ => BobBuilder.CreateImportedPollingStation(jobId2)))
              .ToList();

        var spec = new ListImportedPollingStationsSpecification(jobId1, pagination: new Pagination(2, pageSize: 10));
        var result = spec.Evaluate(testCollection).ToList();

        result.Should().HaveCount(2);
        result.Should().Contain(pollingStation1);
        result.Should().Contain(pollingStation2);
    }
}
