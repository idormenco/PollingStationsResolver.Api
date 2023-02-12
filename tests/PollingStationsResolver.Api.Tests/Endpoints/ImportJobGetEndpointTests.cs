using FastEndpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PollingStationsResolver.Api.Features.ImportJob;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;
using PollingStationsResolver.Domain.Repository;
using Endpoint = PollingStationsResolver.Api.Features.ImportJob.Get.Endpoint;

namespace PollingStationsResolver.Api.Tests.Endpoints;

public class ImportJobGetEndpointTests
{
    private readonly IRepository<ImportJob> _repository = Substitute.For<IRepository<ImportJob>>();
    private readonly Endpoint _endpoint;
    private readonly Guid _jobId = Guid.NewGuid();

    public ImportJobGetEndpointTests()
    {
        _endpoint = Factory.Create<Endpoint>(ctx => ctx.Request.RouteValues.Add("id", _jobId), _repository);
        _endpoint.Map = new ResponseMapper();
    }

    [Fact]
    public async Task CallsRepository_With_RequestedId()
    {
        await _endpoint.HandleAsync(CancellationToken.None);

        await _repository
            .Received(1)
            .GetByIdAsync(_jobId);
    }

    [Fact]
    public async Task ReturnImportJob_When_ExistsInRepository()
    {
        var importJob = BobBuilder.CreateImportJob();

        _repository
            .GetByIdAsync(_jobId)
            .Returns(importJob);

        await _endpoint.HandleAsync(CancellationToken.None);

        _endpoint.ValidationFailed.Should().BeFalse();
        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        _endpoint.Response.Should().BeEquivalentTo(importJob, c => c.Excluding(x => x.File));
    }

    [Fact]
    public async Task ReturnNotFound_When_NotExistsInRepository()
    {
        await _endpoint.HandleAsync(CancellationToken.None);

        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}
