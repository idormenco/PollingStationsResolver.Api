using FastEndpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PollingStationsResolver.Api.Features.PollingStation;
using PollingStationsResolver.Api.Features.PollingStation.Update;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.PollingStationAggregate;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using Endpoint = PollingStationsResolver.Api.Features.PollingStation.Update.Endpoint;

namespace PollingStationsResolver.Api.Tests.Endpoints;

public class PollingStationUpdateEndpointTests
{
    private readonly IRepository<PollingStation> _repository = Substitute.For<IRepository<PollingStation>>();
    private readonly Endpoint _endpoint;

    public PollingStationUpdateEndpointTests()
    {
        _endpoint = Factory.Create<Endpoint>(_repository, new ResponseMapper());
    }

    [Fact]
    public async Task CallsRepository_With_CorrectSpecification()
    {
        var request = new UpdatePollingStationRequest
        {
            Id = Guid.NewGuid()
        };

        await _endpoint.HandleAsync(request, CancellationToken.None);

        await _repository
            .Received(1)
            .FirstOrDefaultAsync(Arg.Any<GetPollingStationSpecification>());
    }

    [Fact]
    public async Task UpdatePollingStation_When_ExistsInRepository()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existingPollingStation = BobBuilder.CreatePollingStation(id);

        var request = BobBuilder.CreateUpdatePollingStationRequest(id);

        request = request with
        {
            AssignedAddresses = new[]
            {
                // first address will be updated
                BobBuilder.CreateUpdateAssignedAddressRequest(existingPollingStation.AssignedAddresses.First().Id),
                // this address will be added
                BobBuilder.CreateUpdateAssignedAddressRequest()
            }
        };

        _repository
            .FirstOrDefaultAsync(Arg.Any<GetPollingStationSpecification>())
            .Returns(existingPollingStation);

        PollingStation? updatedPollingStation = null;
        await _repository.UpdateAsync(Arg.Do<PollingStation>(x => { updatedPollingStation = x; }));

        // Act
        await _endpoint.HandleAsync(request, CancellationToken.None);

        // Act
        updatedPollingStation.Should().NotBeNull();
        updatedPollingStation.Should().BeEquivalentTo(request, c => c.Excluding(x => x.AssignedAddresses[1].Id));
        updatedPollingStation!.AssignedAddresses.Last().Id.Should().BeEmpty("New addresses will have empty Ids");
    }

    [Fact]
    public async Task ReturnUpdatedPollingStation_AfterUpdate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existingPollingStation = BobBuilder.CreatePollingStation(id);

        var request = BobBuilder.CreateUpdatePollingStationRequest(id) with
        {
            AssignedAddresses = new[]
            {
                BobBuilder.CreateUpdateAssignedAddressRequest(existingPollingStation.AssignedAddresses.First().Id),
                BobBuilder.CreateUpdateAssignedAddressRequest(existingPollingStation.AssignedAddresses.Last().Id)
            }
        };

        _repository
            .FirstOrDefaultAsync(Arg.Any<GetPollingStationSpecification>())
            .Returns(existingPollingStation);

        // Act
        await _endpoint.HandleAsync(request, CancellationToken.None);

        // Act
        existingPollingStation.Should().NotBeNull();
        existingPollingStation.Should().BeEquivalentTo(request);
    }

    [Fact]
    public async Task ReturnNotFound_When_NotExistsInRepository()
    {
        var request = BobBuilder.CreateUpdatePollingStationRequest();

        await _endpoint.HandleAsync(request, CancellationToken.None);

        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}
