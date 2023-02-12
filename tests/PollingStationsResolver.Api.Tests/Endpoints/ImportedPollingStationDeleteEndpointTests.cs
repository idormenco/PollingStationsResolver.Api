using FastEndpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PollingStationsResolver.Api.Features.ImportedPollingStation.Delete;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using Endpoint = PollingStationsResolver.Api.Features.ImportedPollingStation.Delete.Endpoint;

namespace PollingStationsResolver.Api.Tests.Endpoints;

public class ImportedPollingStationDeleteEndpointTests
{
    private readonly IRepository<ImportedPollingStation> _repository = Substitute.For<IRepository<ImportedPollingStation>>();
    private readonly Endpoint _endpoint;

    public ImportedPollingStationDeleteEndpointTests()
    {
        _endpoint = Factory.Create<Endpoint>(_repository);
    }

    [Fact]
    public async Task CallsRepository_With_CorrectSpecification()
    {
        var request = new Request
        {
            Id = Guid.NewGuid()
        };

        await _endpoint.HandleAsync(request, CancellationToken.None);

        await _repository
            .Received(1)
            .FirstOrDefaultAsync(Arg.Any<GetImportedPollingStationSpecification>());
    }

    [Fact]
    public async Task DeleteImportedPollingStation_When_ExistsInRepository()
    {
        var id = Guid.NewGuid();
        var jobId = Guid.NewGuid();
        var request = new Request
        {
            Id = id,
            JobId = jobId
        };

        var importedPollingStation = BobBuilder.CreateImportedPollingStation(jobId, id);

        _repository
            .FirstOrDefaultAsync(Arg.Any<GetImportedPollingStationSpecification>())
            .Returns(importedPollingStation);

        await _endpoint.HandleAsync(request, CancellationToken.None);

        await _repository
            .Received(1)
            .DeleteAsync(Arg.Is<ImportedPollingStation>(x => x.Id == id));

        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task ReturnNotFound_When_NotExistsInRepository()
    {
        var request = new Request
        {
            Id = Guid.NewGuid()
        };

        await _endpoint.HandleAsync(request, CancellationToken.None);

        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }


}
