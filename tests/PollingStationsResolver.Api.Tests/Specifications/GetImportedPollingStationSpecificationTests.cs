using FluentAssertions;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Specifications;

namespace PollingStationsResolver.Api.Tests.Specifications;

public class GetImportedPollingStationSpecificationTests
{
    [Fact]
    public void GetImportedPollingStationSpecification_AppliesCorrectFilters()
    {
        var jobId = Guid.NewGuid();
        var id = Guid.NewGuid();

        var requestedImportedPollingStation = BobBuilder.CreateImportedPollingStation(jobId,id);

        var testCollection = new List<ImportedPollingStation>()
        {
            requestedImportedPollingStation,
            BobBuilder.CreateImportedPollingStation(Guid.NewGuid(), id),
            BobBuilder.CreateImportedPollingStation(jobId, Guid.NewGuid()),
            BobBuilder.CreateImportedPollingStation(Guid.NewGuid(), Guid.NewGuid()),
        };

        var spec = new GetImportedPollingStationSpecification(jobId, id);

        var result = spec.Evaluate(testCollection).FirstOrDefault();

        result.Should().BeEquivalentTo(requestedImportedPollingStation);
    }
}
