using FastEndpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PollingStationsResolver.Api.Features.ImportedPollingStation;
using PollingStationsResolver.Api.Features.ImportedPollingStation.Update;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using Endpoint = PollingStationsResolver.Api.Features.ImportedPollingStation.Update.Endpoint;

namespace PollingStationsResolver.Api.Tests.Endpoints;

public class ImportedPollingStationUpdateEndpointTests
{
    private readonly IRepository<ImportedPollingStation> _repository = Substitute.For<IRepository<ImportedPollingStation>>();
    private readonly Endpoint _endpoint;

    public ImportedPollingStationUpdateEndpointTests()
    {
        _endpoint = Factory.Create<Endpoint>(_repository, new ResponseMapper());
    }

    [Fact]
    public async Task CallsRepository_With_CorrectSpecification()
    {
        var request = new UpdateImportedPollingStationRequest
        {
            Id = Guid.NewGuid()
        };

        await _endpoint.HandleAsync(request, CancellationToken.None);

        await _repository
            .Received(1)
            .FirstOrDefaultAsync(Arg.Any<GetImportedPollingStationSpecification>());
    }

    [Fact]
    public async Task UpdateImportedPollingStation_When_ExistsInRepository()
    {
        // Arrange
        var existingImportedPollingStation = BobBuilder.CreateImportedPollingStation();

        var request = BobBuilder.CreateImportedUpdatePollingStationRequest(existingImportedPollingStation.Id);

        request = request with
        {
            JobId = existingImportedPollingStation.JobId,
            AssignedAddresses = new[]
            {
                // first address will be updated
                BobBuilder.CreateUpdateAssignedAddressRequest(existingImportedPollingStation.AssignedAddresses.First().Id),
                // this address will be added
                BobBuilder.CreateUpdateAssignedAddressRequest()
            }
        };

        _repository
            .FirstOrDefaultAsync(Arg.Any<GetImportedPollingStationSpecification>())
            .Returns(existingImportedPollingStation);

        ImportedPollingStation? updatedImportedPollingStation = null;
        await _repository.UpdateAsync(Arg.Do<ImportedPollingStation>(x => { updatedImportedPollingStation = x; }));

        // Act
        await _endpoint.HandleAsync(request, CancellationToken.None);

        // Act
        updatedImportedPollingStation.Should().NotBeNull();
        updatedImportedPollingStation.Should().BeEquivalentTo(request, c => c.Excluding(x => x.AssignedAddresses[1].Id));
        updatedImportedPollingStation!.AssignedAddresses.Last().Id.Should().BeEmpty("New addresses will have empty Ids");
    }

    [Fact]
    public async Task ReturnImportedUpdatedPollingStation_AfterUpdate()
    {
        // Arrange
        var existingImportedPollingStation = BobBuilder.CreateImportedPollingStation();

        var request = BobBuilder.CreateImportedUpdatePollingStationRequest(existingImportedPollingStation.Id) with
        {
            JobId = existingImportedPollingStation.JobId,
            AssignedAddresses = new[]
            {
                BobBuilder.CreateUpdateAssignedAddressRequest(existingImportedPollingStation.AssignedAddresses.First().Id),
                BobBuilder.CreateUpdateAssignedAddressRequest(existingImportedPollingStation.AssignedAddresses.Last().Id)
            }
        };

        _repository
            .FirstOrDefaultAsync(Arg.Any<GetImportedPollingStationSpecification>())
            .Returns(existingImportedPollingStation);

        // Act
        await _endpoint.HandleAsync(request, CancellationToken.None);

        // Act
        existingImportedPollingStation.Should().NotBeNull();
        existingImportedPollingStation.Should().BeEquivalentTo(request);
    }

    [Fact]
    public async Task ReturnNotFound_When_NotExistsInRepository()
    {
        var request = BobBuilder.CreateImportedUpdatePollingStationRequest();

        await _endpoint.HandleAsync(request, CancellationToken.None);

        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}
