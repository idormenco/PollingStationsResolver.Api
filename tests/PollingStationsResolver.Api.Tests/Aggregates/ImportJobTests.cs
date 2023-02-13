using FluentAssertions;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Api.Tests.Aggregates;

public class ImportJobTests
{

    [Fact]
    public void ImportJob_Constructor_ShouldSetCorrectValues()
    {
        // Arrange
        var importJob = new ImportJob("test_file.csv", "test_file");

        // Assert
        importJob.JobStatus.Should().Be(ImportJobStatus.NotStarted);
        importJob.StartedAt.Should().BeNull();
        importJob.FinishedAt.Should().BeNull();
        importJob.FileId.Should().BeEmpty();
        importJob.FileName.Should().Be("test_file.csv");
    }

    [Fact]
    public void Start_ShouldSetCorrectValues()
    {
        // Arrange
        var importJob = new ImportJob("test_file.csv", "test_file");

        // Act
        importJob.Start();

        // Assert
        importJob.JobStatus.Should().Be(ImportJobStatus.Started);
        importJob.StartedAt.Should().NotBeNull();
        importJob.FinishedAt.Should().BeNull();
    }

    [Fact]
    public void End_ShouldSetCorrectValues()
    {
        // Arrange
        var importJob = new ImportJob();
        importJob.Start();

        // Act
        importJob.End();

        // Assert
        importJob.JobStatus.Should().Be(ImportJobStatus.Finished);
        importJob.FinishedAt.Should().NotBeNull();
    }

    [Fact]
    public void Cancel_ShouldSetCorrectValues()
    {
        // Arrange
        var importJob = new ImportJob();

        // Act
        importJob.Cancel();

        // Assert
        importJob.JobStatus.Should().Be(ImportJobStatus.Canceled);
    }
}
