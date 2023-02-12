using FastEndpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;
using PollingStationsResolver.Domain.Repository;
using Endpoint = PollingStationsResolver.Api.Features.ImportJob.Cancel.Endpoint;

namespace PollingStationsResolver.Api.Tests.Endpoints;

public class ImportJobCancelEndpointTests
{
    private readonly IRepository<ImportJob> _repository = Substitute.For<IRepository<ImportJob>>();
    private readonly Endpoint _endpoint;
    private readonly Guid _jobId = Guid.NewGuid();

    public ImportJobCancelEndpointTests()
    {
        _endpoint = Factory.Create<Endpoint>(ctx => ctx.Request.RouteValues.Add("id", _jobId), _repository);
    }

    [Fact]
    public async Task CallsRepository_With_RouteId()
    {
        await _endpoint.HandleAsync(CancellationToken.None);

        await _repository
            .Received(1)
            .GetByIdAsync(Arg.Is(_jobId));
    }

    [Fact]
    public async Task Changes_ImportJobStatus_To_Cancelled()
    {
        // Arrange
        _repository
            .GetByIdAsync(_jobId)
            .Returns(BobBuilder.CreateImportJob(_jobId));

        ImportJob? updatedImportJob = null;
        await _repository.UpdateAsync(Arg.Do<ImportJob>(x => { updatedImportJob = x; }));

        // Act
        await _endpoint.HandleAsync(CancellationToken.None);

        // Act
        updatedImportJob.Should().NotBeNull();
        updatedImportJob!.JobStatus.Should().Be(ImportJobStatus.Canceled);
    }

    [Fact]
    public async Task ReturnNoContent_When_UpdateSuccessful()
    {
        // Arrange
        _repository
            .GetByIdAsync(_jobId)
            .Returns(BobBuilder.CreateImportJob(_jobId));

        // Act
        await _endpoint.HandleAsync(CancellationToken.None);

        // Asset
        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task ReturnNotFound_When_NotExistsInRepository()
    {
        // Act
        await _endpoint.HandleAsync(CancellationToken.None);

        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}
