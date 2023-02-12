using FluentAssertions;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;
using PollingStationsResolver.Domain.Specifications;

namespace PollingStationsResolver.Api.Tests.Specifications;

public class CurrentImportJobInProgressSpecificationTests
{
    [Theory]
    [MemberData(nameof(InProgressJobTestCases))]
    public void CurrentImportJobInProgressSpecification_Returns_JobInProgress(ImportJobStatus status)
    {
        var spec = new CurrentImportJobInProgressSpecification();

        var inProgressImportJob = BobBuilder.CreateImportJob(status: status);
        var finishedImportJob = BobBuilder.CreateImportJob(status: ImportJobStatus.Finished);

        var testCollection = new List<ImportJob>()
        {
            inProgressImportJob,
            finishedImportJob
        };

        var result = spec.Evaluate(testCollection).FirstOrDefault();

        result.Should().BeEquivalentTo(inProgressImportJob);
    }

    [Theory]
    [MemberData(nameof(FinishedJobTestCases))]
    public void CurrentImportJobInProgressSpecification_Ignores_FinishedJobs(ImportJobStatus status)
    {
        var spec = new CurrentImportJobInProgressSpecification();

        var finishedImportJob = BobBuilder.CreateImportJob(status: status);

        var testCollection = new List<ImportJob>()
        {
            finishedImportJob
        };

        var result = spec.Evaluate(testCollection).FirstOrDefault();

        result.Should().BeNull();
    }

    public static IEnumerable<object[]> InProgressJobTestCases =>
        new List<object[]>
        {
            new object[] { ImportJobStatus.Started},
            new object[] { ImportJobStatus.NotStarted},
            new object[] { ImportJobStatus.Finished}
        };

    public static IEnumerable<object[]> FinishedJobTestCases =>
        new List<object[]>
        {
            new object[] { ImportJobStatus.Canceled},
            new object[] { ImportJobStatus.Imported}
        };
}
