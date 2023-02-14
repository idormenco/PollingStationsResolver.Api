using FluentAssertions;
using NSubstitute;
using PollingStationsResolver.Api.HangfireJobs;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using PollingStationsResolver.Geocoding;
using PollingStationsResolver.Geocoding.Models;

namespace PollingStationsResolver.Api.Tests.HangfireJobs;

public class GeocodingJobTests
{
    private readonly IRepository<ImportJob> _importJobRepository;
    private readonly IRepository<ImportedPollingStation> _importedPollingStationRepository;
    private readonly IGetAddressCoordinatesQuery _getAddressCoordinatesQuery;
    private readonly GeocodingJob _geocodingJob;

    public GeocodingJobTests()
    {
        _importJobRepository = Substitute.For<IRepository<ImportJob>>();
        _importedPollingStationRepository = Substitute.For<IRepository<ImportedPollingStation>>();
        _getAddressCoordinatesQuery = Substitute.For<IGetAddressCoordinatesQuery>();
        _geocodingJob = new GeocodingJob(_importJobRepository, _importedPollingStationRepository, _getAddressCoordinatesQuery);
    }

    [Fact]
    public async Task Run_WhenImportJobIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var jobId = Guid.NewGuid();
        var pollingStationId = Guid.NewGuid();
        ImportJob? importJob = null;

        _importJobRepository.GetByIdAsync(jobId).Returns(importJob);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _geocodingJob.Run(jobId, pollingStationId, CancellationToken.None));
    }

    [Fact]
    public async Task Run_WhenImportJobIsCanceled_ShouldReturnImmediately()
    {
        // Arrange
        var jobId = Guid.NewGuid();
        var pollingStationId = Guid.NewGuid();
        var importJob = BobBuilder.CreateImportJob(jobId, ImportJobStatus.Canceled);

        _importJobRepository.GetByIdAsync(jobId).Returns(importJob);

        // Act
        await _geocodingJob.Run(jobId, pollingStationId, CancellationToken.None);

        // Assert
        await _importedPollingStationRepository.DidNotReceive().FirstOrDefaultAsync(Arg.Any<GetImportedPollingStationSpecification>(),
            Arg.Any<CancellationToken>());
        await _getAddressCoordinatesQuery
            .DidNotReceive()
            .ExecuteAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Run_WhenImportedPollingStationIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var jobId = Guid.NewGuid();
        var pollingStationId = Guid.NewGuid();
        var importJob = BobBuilder.CreateImportJob(jobId, ImportJobStatus.Started);
        ImportedPollingStation? importedPollingStation = null;

        _importJobRepository.GetByIdAsync(jobId).Returns(importJob);
        _importedPollingStationRepository.FirstOrDefaultAsync(Arg.Any<GetImportedPollingStationSpecification>())
            .Returns(importedPollingStation);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _geocodingJob.Run(jobId, pollingStationId, CancellationToken.None));
    }

    [Fact]
    public async Task Run_WhenResolvedAddressStatusIsSuccess_ShouldReturnImmediately()
    {
        // Arrange
        var jobId = Guid.NewGuid();
        var pollingStationId = Guid.NewGuid();
        var importJob = BobBuilder.CreateImportJob(jobId, ImportJobStatus.Started);
        var importedPollingStation = BobBuilder.CreateImportedPollingStation(jobId, pollingStationId, ResolvedAddressStatus.Success);

        _importJobRepository.GetByIdAsync(jobId).Returns(importJob);

        _importedPollingStationRepository
            .FirstOrDefaultAsync(Arg.Any<GetImportedPollingStationSpecification>())
            .Returns(importedPollingStation);

        // Act
        await _geocodingJob.Run(jobId, pollingStationId, CancellationToken.None);

        // Assert
        await _getAddressCoordinatesQuery
            .DidNotReceive()
            .ExecuteAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Run_WithUnresolvedPollingStationAddressAndFoundResult_UpdatesPollingStationCoordinates()
    {
        // Arrange
        var jobId = Guid.NewGuid();
        var pollingStationId = Guid.NewGuid();

        // Create an import job
        var importJob = BobBuilder.CreateImportJob(jobId, ImportJobStatus.Started);
        _importJobRepository.GetByIdAsync(jobId).Returns(importJob);

        // Create an unresolved polling station
        var unresolvedPollingStation = BobBuilder.CreateImportedPollingStation(jobId, pollingStationId, ResolvedAddressStatus.NotProcessed);
        _importedPollingStationRepository.FirstOrDefaultAsync(Arg.Any<GetImportedPollingStationSpecification>()).Returns(unresolvedPollingStation);

        // Create a found result
        var latitude = 37.7749;
        var longitude = -122.4194;
        var result = new LocationSearchResult.Found(latitude, longitude);
        _getAddressCoordinatesQuery.ExecuteAsync(unresolvedPollingStation.County, unresolvedPollingStation.Address).Returns(result);

        // Act
        await _geocodingJob.Run(jobId, pollingStationId, CancellationToken.None);

        // Assert
        unresolvedPollingStation.Latitude.Should().Be(latitude);
        unresolvedPollingStation.Longitude.Should().Be(longitude);
        unresolvedPollingStation.ResolvedAddressStatus.Should().Be(ResolvedAddressStatus.Success);
        await _importedPollingStationRepository.Received(1).UpdateAsync(unresolvedPollingStation);
    }

    [Fact]
    public async Task Run_WithUnresolvedPollingStationAddressAndNotFoundResult_MarksAsNotFound()
    {
        // Arrange
        var jobId = Guid.NewGuid();
        var pollingStationId = Guid.NewGuid();

        // Create an import job
        var importJob = BobBuilder.CreateImportJob(jobId, ImportJobStatus.Started);
        _importJobRepository.GetByIdAsync(jobId).Returns(importJob);

        // Create an unresolved polling station
        var unresolvedPollingStation = BobBuilder.CreateImportedPollingStation(jobId, pollingStationId, ResolvedAddressStatus.NotProcessed);
        _importedPollingStationRepository
            .FirstOrDefaultAsync(Arg.Any<GetImportedPollingStationSpecification>())
            .Returns(unresolvedPollingStation);

        // Create a not found result
        var locationSearchResult = new LocationSearchResult.NotFound();
        _getAddressCoordinatesQuery
            .ExecuteAsync(unresolvedPollingStation.County, unresolvedPollingStation.Address)
            .Returns(locationSearchResult);

        // Act
        await _geocodingJob.Run(jobId, pollingStationId, CancellationToken.None);

        // Assert
        unresolvedPollingStation.Latitude.Should().BeNull();
        unresolvedPollingStation.Longitude.Should().BeNull();
        unresolvedPollingStation.ResolvedAddressStatus.Should().Be(ResolvedAddressStatus.NotFound);
        await _importedPollingStationRepository.Received(1).UpdateAsync(unresolvedPollingStation);
    }

    [Fact]
    public async Task Run_WithUnresolvedPollingStationAddressAndErrorResult_DoesNotUpdatePollingStation()
    {
        // Arrange
        var jobId = Guid.NewGuid();
        var pollingStationId = Guid.NewGuid();

        // Create an import job
        var importJob = BobBuilder.CreateImportJob(jobId, ImportJobStatus.Started);
        _importJobRepository.GetByIdAsync(jobId).Returns(importJob);

        // Create an unresolved polling station
        var unresolvedPollingStation = BobBuilder.CreateImportedPollingStation(jobId, pollingStationId, ResolvedAddressStatus.NotProcessed);
        _importedPollingStationRepository
            .FirstOrDefaultAsync(Arg.Any<GetImportedPollingStationSpecification>())
            .Returns(unresolvedPollingStation);

        // Create a not found result
        var locationSearchResult = new LocationSearchResult.Error();
        _getAddressCoordinatesQuery
            .ExecuteAsync(unresolvedPollingStation.County, unresolvedPollingStation.Address)
            .Returns(locationSearchResult);

        // Act
        await _geocodingJob.Run(jobId, pollingStationId, CancellationToken.None);

        // Assert
        unresolvedPollingStation.Latitude.Should().BeNull();
        unresolvedPollingStation.Longitude.Should().BeNull();
        unresolvedPollingStation.ResolvedAddressStatus.Should().Be(ResolvedAddressStatus.NotProcessed);
        await _importedPollingStationRepository.DidNotReceive().UpdateAsync(Arg.Any<ImportedPollingStation>());
    }
}
