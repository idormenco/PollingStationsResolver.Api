using FastEndpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PollingStationsResolver.Api.Features.PollingStation;
using PollingStationsResolver.Api.Features.PollingStation.List;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.PollingStationAggregate;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using Endpoint = PollingStationsResolver.Api.Features.PollingStation.List.Endpoint;

namespace PollingStationsResolver.Api.Tests.Endpoints;

public class PollingStationListEndpointTests
{
    private readonly IRepository<PollingStation> _repository = Substitute.For<IRepository<PollingStation>>();
    private readonly Endpoint _endpoint;

    public PollingStationListEndpointTests()
    {
        _endpoint = Factory.Create<Endpoint>(_repository);
        _endpoint.Map = new ResponseMapper();

    }

    [Fact]
    public async Task CallsRepository_With_CorrectSpecification()
    {
        _repository
            .ListAsync(Arg.Any<ListPollingStationSpecification>())
            .Returns(new List<PollingStation>());

        var request = new Request
        {
            Page = 1,
            PageSize = 10
        };

        await _endpoint.HandleAsync(request, CancellationToken.None);

        await _repository
            .Received(1)
            .ListAsync(Arg.Any<ListPollingStationSpecification>());
    }

    [Fact]
    public async Task ReturnPollingStation_When_ExistsInRepository()
    {
        var request = new Request
        {
            Page = 1,
            PageSize = 10
        };

        var pollingStation1 = BobBuilder.CreatePollingStation();
        var pollingStation2 = BobBuilder.CreatePollingStation();
        var pollingStations = new List<PollingStation> { pollingStation1, pollingStation2 };

        _repository
            .ListAsync(Arg.Any<ListPollingStationSpecification>())
            .Returns(pollingStations);

        await _endpoint.HandleAsync(request, CancellationToken.None);

        _endpoint.ValidationFailed.Should().BeFalse();
        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        _endpoint.Response.Should().BeEquivalentTo(pollingStations);
    }

    [Fact]
    public async Task ReturnEmptyList_When_NoneInRepository()
    {
        _repository
            .ListAsync(Arg.Any<ListPollingStationSpecification>())
            .Returns(new List<PollingStation>());

        var request = new Request
        {
            Page = 1,
            PageSize = 10
        };

        await _endpoint.HandleAsync(request, CancellationToken.None);
        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
}
