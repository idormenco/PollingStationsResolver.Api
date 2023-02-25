using FluentAssertions;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;
using PollingStationsResolver.Domain.Specifications;

namespace PollingStationsResolver.Api.Tests.Specifications;

public class ListImportedPollingStationsWithUnresolvedAddressSpecificationTests
{
    [Fact]
    public void ListImportedPollingStationsWithUnresolvedAddressSpecification_AppliesCorrectFilters()
    {
        Guid jobId = Guid.NewGuid();

        var unresolvedPollingStation1 = BobBuilder.CreateImportedPollingStation(jobId: jobId,status: ResolvedAddressStatus.NotFound);
        var unresolvedPollingStation2 = BobBuilder.CreateImportedPollingStation(jobId: jobId,status: ResolvedAddressStatus.NotProcessed);
        
        var testCollection = new List<ImportedPollingStation>()
        {
            BobBuilder.CreateImportedPollingStation(jobId: jobId,status: ResolvedAddressStatus.Success),
            unresolvedPollingStation1,
            unresolvedPollingStation2
        };


        var spec = new ListImportedPollingStationsWithUnresolvedAddressSpecification(jobId);

        var result = spec.Evaluate(testCollection).ToList();

        result.Should().HaveCount(2);
        result.Should().Contain(unresolvedPollingStation1);
        result.Should().Contain(unresolvedPollingStation2);
    }
}
