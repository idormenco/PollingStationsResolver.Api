
using FastEndpoints;
using FluentAssertions;
using NSubstitute;
using PollingStationsResolver.Api.Features.PollingStation;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.PollingStationAggregate;
using PollingStationsResolver.Domain.Repository;
using Endpoint = PollingStationsResolver.Api.Features.PollingStation.Add.Endpoint;

namespace PollingStationsResolver.Api.Tests.Endpoints;

public class PollingStationAddEndpointTests
{
    private readonly IRepository<PollingStation> _repository = Substitute.For<IRepository<PollingStation>>();
    private readonly Endpoint _endpoint;

    public PollingStationAddEndpointTests()
    {
        _endpoint = Factory.Create<Endpoint>(_repository, new ResponseMapper());
    }

    [Fact]
    public async Task Should_AddPollingStation_With_RequestedData()
    {
        // Arrange
        var request = BobBuilder.CreateAddPollingStationRequest();
        PollingStation? addedPollingStation = null;

        _repository
            .AddAsync(Arg.Do<PollingStation>(x => addedPollingStation = x))
            .Returns(BobBuilder.CreatePollingStation());

        // Act
        await _endpoint.HandleAsync(request, CancellationToken.None);

        // Asset
        addedPollingStation.Should().BeEquivalentTo(request);
    }

    [Fact]
    public async Task MapToResponseCorrectly()
    {
        var request = BobBuilder.CreateAddPollingStationRequest();
        var createdPollingStation = BobBuilder.CreatePollingStation();

        _repository.AddAsync(Arg.Any<PollingStation>()).Returns(createdPollingStation);
        await _endpoint.HandleAsync(request, CancellationToken.None);

        _endpoint.Response.Should().BeEquivalentTo(createdPollingStation);
    }
}
