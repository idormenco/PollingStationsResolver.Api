using FastEndpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PollingStationsResolver.Api.Features.ImportJob;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using Endpoint = PollingStationsResolver.Api.Features.ImportJob.GetCurrentInProgress.Endpoint;

namespace PollingStationsResolver.Api.Tests.Endpoints;

public class ImportJobGetCurrentInProgressEndpointTests
{
    private readonly IRepository<ImportJob> _repository = Substitute.For<IRepository<ImportJob>>();
    private readonly Endpoint _endpoint;

    public ImportJobGetCurrentInProgressEndpointTests()
    {
        _endpoint = Factory.Create<Endpoint>(_repository);
        _endpoint.Map = new ResponseMapper();
    }


    [Fact]
    public async Task CallsRepository_With_CorrectSpecification()
    {
        await _endpoint.HandleAsync(CancellationToken.None);

        await _repository
            .Received(1)
            .FirstOrDefaultAsync(Arg.Any<CurrentImportJobInProgressSpecification>());
    }

    [Fact]
    public async Task ReturnImportJob_When_ExistsInRepository()
    {
        // Arrange
        var importJob = BobBuilder.CreateImportJob();

        _repository
            .FirstOrDefaultAsync(Arg.Any<CurrentImportJobInProgressSpecification>())
            .Returns(importJob);

        // Act
        await _endpoint.HandleAsync(CancellationToken.None);

        // Assert
        _endpoint.ValidationFailed.Should().BeFalse();
        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        _endpoint.Response.Should().BeEquivalentTo(importJob, c=>c.Excluding(x=>x.File));
    }

    [Fact]
    public async Task ReturnNotFound_When_NotExistsInRepository()
    {
        await _endpoint.HandleAsync(CancellationToken.None);

        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}
