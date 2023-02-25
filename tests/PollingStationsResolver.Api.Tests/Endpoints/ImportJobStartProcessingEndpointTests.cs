using System.Collections.Immutable;
using System.Linq.Expressions;
using FastEndpoints;
using FluentAssertions;
using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PollingStationsResolver.Api.HangfireJobs;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using Endpoint = PollingStationsResolver.Api.Features.ImportJob.StartProcessing.Endpoint;

namespace PollingStationsResolver.Api.Tests.Endpoints;

public class ImportJobStartProcessingEndpointTests
{
    private readonly IRepository<ImportJob> _importJobRepository;
    private readonly IRepository<ImportedPollingStation> _repository;
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly Endpoint _endpoint;
    private readonly Guid _jobId;

    public ImportJobStartProcessingEndpointTests()
    {
        _importJobRepository = Substitute.For<IRepository<ImportJob>>();
        _repository = Substitute.For<IRepository<ImportedPollingStation>>();
        _backgroundJobClient = Substitute.For<IBackgroundJobClient>();
        _jobId = Guid.NewGuid();

        _endpoint = Factory.Create<Endpoint>(ctx => ctx.Request.RouteValues.Add("id", _jobId), _repository, _importJobRepository, _backgroundJobClient);
    }

    [Fact]
    public async Task HandleAsync_WithExistingImportJob_StartsJobAndEnqueuesGeocodingJobs()
    {
        // Arrange
        var importJob = BobBuilder.CreateImportJob(_jobId);
        var importedPollingStations = new List<ImportedPollingStation>
        {
            BobBuilder.CreateImportedPollingStation(_jobId),
            BobBuilder.CreateImportedPollingStation(_jobId),
            BobBuilder.CreateImportedPollingStation(_jobId)
        };

        _importJobRepository
            .GetByIdAsync(_jobId, Arg.Any<CancellationToken>())
            .Returns(importJob);

        _repository
            .ListAsync(Arg.Any<ListImportedPollingStationsSpecification>(), Arg.Any<CancellationToken>())
            .Returns(importedPollingStations);

        // Act
        await _endpoint.HandleAsync(CancellationToken.None);

        // Assert
        await _importJobRepository
            .Received()
            .UpdateAsync(importJob, Arg.Any<CancellationToken>());

        _backgroundJobClient
            .Received(1)
            .Create(Arg.Is<Job>(job => job.Type == typeof(IGeocodingJob) && job.Method.Name == "Run" && (Guid)job.Args[0] == _jobId && (Guid)job.Args[1] == importedPollingStations[0].Id), Arg.Any<IState>());

        _backgroundJobClient
            .Received(1)
            .Create(Arg.Is<Job>(job => job.Type == typeof(IGeocodingJob) && job.Method.Name == "Run" && (Guid)job.Args[0] == _jobId && (Guid)job.Args[1] == importedPollingStations[1].Id), Arg.Any<IState>());

        _backgroundJobClient
            .Received(1)
            .Create(Arg.Is<Job>(job => job.Type == typeof(IGeocodingJob) && job.Method.Name == "Run" && (Guid)job.Args[0] == _jobId && (Guid)job.Args[1] == importedPollingStations[2].Id), Arg.Any<IState>());
    }

    [Fact]
    public async Task HandleAsync_WithNonExistingImportJob_ReturnsNotFound()
    {
        // Act
        await _endpoint.HandleAsync(CancellationToken.None);

        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task HandleAsync_WithExistingImportJob_ReturnsNoContent()
    {
        // Arrange
        var importJob = BobBuilder.CreateImportJob(_jobId);

        _importJobRepository
            .GetByIdAsync(_jobId)
            .Returns(importJob);

        _repository
            .ListAsync(Arg.Any<ListImportedPollingStationsSpecification>(), Arg.Any<CancellationToken>())
            .Returns(new List<ImportedPollingStation>());
        // Act
        await _endpoint.HandleAsync(CancellationToken.None);

        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }
}
