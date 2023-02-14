using FluentAssertions;
using NSubstitute;
using PollingStationsResolver.Api.HangfireJobs;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;

namespace PollingStationsResolver.Api.Tests.HangfireJobs;

public class ImportJobStatusUpdaterJobTests
{
    private readonly IRepository<ImportJob> _repository;
    private readonly IReadRepository<ImportedPollingStation> _importedPollingStationsRepository;
    private readonly ImportJobStatusUpdaterJob _job;

    public ImportJobStatusUpdaterJobTests()
    {
        _repository = Substitute.For<IRepository<ImportJob>>();
        _importedPollingStationsRepository = Substitute.For<IReadRepository<ImportedPollingStation>>();
        _job = new ImportJobStatusUpdaterJob(_repository, _importedPollingStationsRepository);
    }

    [Fact]
    public async Task Run_WithCurrentImportJobInProgress_NoUnprocessedPollingStations_UpdatesImportJob()
    {
        // Arrange
        var importJob = new ImportJob();

        _repository
            .FirstOrDefaultAsync(Arg.Any<CurrentImportJobInProgressSpecification>())!
            .Returns(Task.FromResult(importJob));

        _importedPollingStationsRepository
            .AnyAsync(Arg.Any<GetImportedPollingStationByAddressStatusSpecification>())
            .Returns(Task.FromResult(false));

        // Act
        await _job.Run(CancellationToken.None);

        // Assert
        await _repository.Received(1).UpdateAsync(importJob);
        importJob.JobStatus.Should().Be(ImportJobStatus.Finished);
    }

    [Fact]
    public async Task Run_WithCurrentImportJobInProgress_UnprocessedPollingStations_DoesNotUpdateImportJob()
    {
        // Arrange
        var importJob = new ImportJob();
        _repository
            .FirstOrDefaultAsync(Arg.Any<CurrentImportJobInProgressSpecification>())!
            .Returns(Task.FromResult(importJob));

        _importedPollingStationsRepository.AnyAsync(Arg.Any<GetImportedPollingStationByAddressStatusSpecification>())
            .Returns(Task.FromResult(true));

        // Act
        await _job.Run(CancellationToken.None);

        // Assert
        await _repository.DidNotReceive().UpdateAsync(Arg.Any<ImportJob>());
    }

    [Fact]
    public async Task Run_NoCurrentImportJobInProgress_DoesNotUpdateImportJob()
    {
        // Arrange
        _repository
            .FirstOrDefaultAsync(Arg.Any<CurrentImportJobInProgressSpecification>())!
            .Returns(Task.FromResult<ImportJob>(null));

        // Act
        await _job.Run(CancellationToken.None);

        // Assert
        await _repository.DidNotReceive().UpdateAsync(Arg.Any<ImportJob>());
    }
}
