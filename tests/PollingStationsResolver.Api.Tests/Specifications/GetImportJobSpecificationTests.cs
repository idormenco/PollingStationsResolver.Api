using FluentAssertions;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;
using PollingStationsResolver.Domain.Specifications;

namespace PollingStationsResolver.Api.Tests.Specifications;

public class GetImportJobSpecificationTests
{
    [Fact]
    public void GetImportJobSpecification_Returns_AppliesCorrectFilters()
    {
        var jobId = Guid.NewGuid();
        var requestedJob = BobBuilder.CreateImportJob(jobId);
        var testCollection = new List<ImportJob>()
        {
            requestedJob,
            BobBuilder.CreateImportJob(),
            BobBuilder.CreateImportJob(),
        };


        var spec = new GetImportJobSpecification(jobId);

        var result = spec.Evaluate(testCollection).FirstOrDefault();

        result.Should().BeEquivalentTo(requestedJob);
    }
}
