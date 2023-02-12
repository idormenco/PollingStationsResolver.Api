using FastEndpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PollingStationsResolver.Api.Features.ImportedPollingStation;
using PollingStationsResolver.Api.Features.ImportedPollingStation.Get;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using Endpoint = PollingStationsResolver.Api.Features.ImportedPollingStation.Get.Endpoint;

namespace PollingStationsResolver.Api.Tests.Endpoints;

public class ImportedPollingStationGetEndpointTests
{
    private readonly IRepository<ImportedPollingStation> _repository = Substitute.For<IRepository<ImportedPollingStation>>();
    private readonly Endpoint _endpoint;

    public ImportedPollingStationGetEndpointTests()
    {
        _endpoint = Factory.Create<Endpoint>(_repository);
        _endpoint.Map = new ResponseMapper();

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
    public async Task ReturnImportedPollingStation_When_ExistsInRepository()
    {
        var request = new Request
        {
            Id = Guid.NewGuid()
        };

        var importedPollingStation = BobBuilder.CreateImportedPollingStation();

        _repository
            .FirstOrDefaultAsync(Arg.Any<GetImportedPollingStationSpecification>())
            .Returns(importedPollingStation);

        await _endpoint.HandleAsync(request, CancellationToken.None);

        _endpoint.ValidationFailed.Should().BeFalse();
        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        _endpoint.Response.Should().BeEquivalentTo(importedPollingStation);
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
