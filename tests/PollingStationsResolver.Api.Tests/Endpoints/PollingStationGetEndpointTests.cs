using FastEndpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PollingStationsResolver.Api.Features.PollingStation;
using PollingStationsResolver.Api.Features.PollingStation.Get;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.PollingStationAggregate;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using Endpoint = PollingStationsResolver.Api.Features.PollingStation.Get.Endpoint;

namespace PollingStationsResolver.Api.Tests.Endpoints;

public class PollingStationGetEndpointTests
{
    private readonly IRepository<PollingStation> _repository = Substitute.For<IRepository<PollingStation>>();
    private readonly Endpoint _endpoint;

    public PollingStationGetEndpointTests()
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
            .FirstOrDefaultAsync(Arg.Any<GetPollingStationSpecification>());
    }

    [Fact]
    public async Task ReturnPollingStation_When_ExistsInRepository()
    {
        var request = new Request
        {
            Id = Guid.NewGuid()
        };

        var pollingStation = BobBuilder.CreatePollingStation();

        _repository
            .FirstOrDefaultAsync(Arg.Any<GetPollingStationSpecification>())
            .Returns(pollingStation);

        await _endpoint.HandleAsync(request, CancellationToken.None);

        _endpoint.ValidationFailed.Should().BeFalse();
        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        _endpoint.Response.Should().BeEquivalentTo(pollingStation);
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
