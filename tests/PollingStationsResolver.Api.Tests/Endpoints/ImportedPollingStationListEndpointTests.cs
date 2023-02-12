using FastEndpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PollingStationsResolver.Api.Features.ImportedPollingStation;
using PollingStationsResolver.Api.Features.ImportedPollingStation.List;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using Endpoint = PollingStationsResolver.Api.Features.ImportedPollingStation.List.Endpoint;

namespace PollingStationsResolver.Api.Tests.Endpoints;

public class ImportedPollingStationListEndpointTests
{
    private readonly IRepository<ImportedPollingStation> _repository = Substitute.For<IRepository<ImportedPollingStation>>();
    private readonly Endpoint _endpoint;

    public ImportedPollingStationListEndpointTests()
    {
        _endpoint = Factory.Create<Endpoint>(_repository);
        _endpoint.Map = new ResponseMapper();
    }

    [Fact]
    public async Task CallsRepository_With_CorrectSpecification()
    {
        var request = new Request
        {
            JobId = Guid.NewGuid(),
            Page = 1,
            PageSize = 10
        };

        _repository
            .ListAsync(Arg.Any<ListImportedPollingStationsSpecification>())
            .Returns(new List<ImportedPollingStation>());

        await _endpoint.HandleAsync(request, CancellationToken.None);

        await _repository
            .Received(1)
            .ListAsync(Arg.Any<ListImportedPollingStationsSpecification>());
    }

    [Fact]
    public async Task ReturnImportedPollingStations_When_ExistsInRepository()
    {
        var request = new Request
        {
            JobId = Guid.NewGuid(),
            Page = 1,
            PageSize = 10
        };

        var importedPollingStation1 = BobBuilder.CreateImportedPollingStation();
        var importedPollingStation2 = BobBuilder.CreateImportedPollingStation();

        var importedPollingStations = new List<ImportedPollingStation>(){
            importedPollingStation1,
            importedPollingStation2
        };

        _repository
            .ListAsync(Arg.Any<ListImportedPollingStationsSpecification>())
            .Returns(importedPollingStations);

        await _endpoint.HandleAsync(request, CancellationToken.None);

        _endpoint.ValidationFailed.Should().BeFalse();
        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        _endpoint.Response.Should().BeEquivalentTo(importedPollingStations);
    }

    [Fact]
    public async Task ReturnEmptyList_When_NoneInRepository()
    {
        _repository
            .ListAsync(Arg.Any<ListImportedPollingStationsSpecification>())
            .Returns(new List<ImportedPollingStation>());

        var request = new Request
        {
            JobId = Guid.NewGuid(),
            Page = 1,
            PageSize = 10
        };

        await _endpoint.HandleAsync(request, CancellationToken.None);

        _endpoint.Response.Should().BeEmpty();
        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
}
