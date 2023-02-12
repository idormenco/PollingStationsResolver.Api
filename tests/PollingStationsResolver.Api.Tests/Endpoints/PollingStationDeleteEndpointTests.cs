using FastEndpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PollingStationsResolver.Api.Features.PollingStation.Delete;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.PollingStationAggregate;
using PollingStationsResolver.Domain.Repository;
using Endpoint = PollingStationsResolver.Api.Features.PollingStation.Delete.Endpoint;

namespace PollingStationsResolver.Api.Tests.Endpoints;

public class PollingStationDeleteEndpointTests
{
    private readonly IRepository<PollingStation> _repository = Substitute.For<IRepository<PollingStation>>();
    private readonly Endpoint _endpoint;

    public PollingStationDeleteEndpointTests()
    {
        _endpoint = Factory.Create<Endpoint>(_repository);
    }

    [Fact]
    public async Task CallsRepository_With_CorrectId()
    {
        var request = new Request
        {
            Id = Guid.NewGuid()
        };

        await _endpoint.HandleAsync(request, CancellationToken.None);

        await _repository
            .Received(1)
            .GetByIdAsync(request.Id);
    }

    [Fact]
    public async Task DeletePollingStation_When_ExistsInRepository()
    {
        var id = Guid.NewGuid();
        var request = new Request
        {
            Id = id
        };

        var pollingStation = BobBuilder.CreatePollingStation(id);

        _repository
            .GetByIdAsync(id)
            .Returns(pollingStation);

        await _endpoint.HandleAsync(request, CancellationToken.None);

        await _repository
            .Received(1)
            .DeleteAsync(Arg.Is<PollingStation>(x => x.Id == id));

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
