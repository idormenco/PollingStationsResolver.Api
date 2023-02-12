
using FastEndpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PollingStationsResolver.Api.Features.ImportedPollingStation;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;
using PollingStationsResolver.Domain.Repository;
using Endpoint = PollingStationsResolver.Api.Features.ImportedPollingStation.Add.Endpoint;

namespace PollingStationsResolver.Api.Tests.Endpoints;

public class ImportedPollingStationAddEndpointTests
{
    private readonly IRepository<ImportedPollingStation> _repository = Substitute.For<IRepository<ImportedPollingStation>>();
    private readonly IRepository<ImportJob> _importJobRepository = Substitute.For<IRepository<ImportJob>>();
    private readonly Endpoint _endpoint;
    private readonly Guid _jobId = Guid.NewGuid();

    public ImportedPollingStationAddEndpointTests()
    {
        _endpoint = Factory.Create<Endpoint>(_repository, _importJobRepository, new ResponseMapper());
    }

    [Fact]
    public async Task Should_AddImportedPollingStation_With_RequestedData()
    {
        // Arrange
        _importJobRepository
            .GetByIdAsync(Arg.Is(_jobId))
            .Returns(BobBuilder.CreateImportJob(_jobId));

        var request = BobBuilder.CreateAddImportedPollingStationRequest(_jobId);

        ImportedPollingStation? addedImportedPollingStation = null;
        _repository
            .AddAsync(Arg.Do<ImportedPollingStation>(x => addedImportedPollingStation = x))
            .Returns(BobBuilder.CreateImportedPollingStation(_jobId));

        // Act
        await _endpoint.HandleAsync(request, CancellationToken.None);

        // Asset
        addedImportedPollingStation.Should().BeEquivalentTo(request);
    }

    [Fact]
    public async Task MapToResponseCorrectly()
    {
        // Arrange
        _importJobRepository
            .GetByIdAsync(Arg.Is(_jobId))
            .Returns(BobBuilder.CreateImportJob(_jobId));

        var request = BobBuilder.CreateAddImportedPollingStationRequest(_jobId);

        var createdPollingStation = BobBuilder.CreateImportedPollingStation();
        _repository.AddAsync(Arg.Any<ImportedPollingStation>()).Returns(createdPollingStation);

        // Act
        await _endpoint.HandleAsync(request, CancellationToken.None);

        // Assert
        _endpoint.Response.Should().BeEquivalentTo(createdPollingStation);
    }

    [Fact]
    public async Task ReturnNotFound_When_NotExistsInRepository()
    {
        // Arrange
        var request = BobBuilder.CreateAddImportedPollingStationRequest(_jobId);

        // Act
        await _endpoint.HandleAsync(request, CancellationToken.None);

        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}
