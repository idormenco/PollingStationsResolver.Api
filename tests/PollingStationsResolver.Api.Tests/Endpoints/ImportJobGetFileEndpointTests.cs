using FastEndpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using Endpoint = PollingStationsResolver.Api.Features.ImportJob.GetFile.Endpoint;

namespace PollingStationsResolver.Api.Tests.Endpoints;

public class ImportJobGetFileEndpointTests
{
    private readonly IRepository<ImportJob> _repository = Substitute.For<IRepository<ImportJob>>();
    private readonly Endpoint _endpoint;
    private readonly Guid _jobId = Guid.NewGuid();

    public ImportJobGetFileEndpointTests()
    {
        _endpoint = Factory.Create<Endpoint>(ctx => ctx.Request.RouteValues.Add("id", _jobId), _repository);
    }

    [Fact]
    public async Task CallsRepository_With_CorrectSpecification()
    {
        await _endpoint.HandleAsync(CancellationToken.None);

        await _repository
            .Received(1)
            .FirstOrDefaultAsync(Arg.Any<GetImportJobSpecification>());
    }

    [Fact]
    public async Task ReturnFile_When_ExistsInRepository()
    {
        ImportJob importJob = BobBuilder.CreateImportJob(_jobId);

        _repository
            .FirstOrDefaultAsync(Arg.Any<GetImportJobSpecification>())
            .Returns(importJob);

        await _endpoint.HandleAsync(CancellationToken.None);

        _endpoint.ValidationFailed.Should().BeFalse();
        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);

        _endpoint.HttpContext.Response.Body.ReadBody()
              .Should()
              .Be("VGhpcyBpcyB0aGUgZXhjZWwgZmlsZSB5b3UgYXJlIGxvb2tpbmcgZm9y");
    }

    [Fact]
    public async Task ReturnNotFound_When_NotExistsInRepository()
    {
        await _endpoint.HandleAsync(CancellationToken.None);

        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}
