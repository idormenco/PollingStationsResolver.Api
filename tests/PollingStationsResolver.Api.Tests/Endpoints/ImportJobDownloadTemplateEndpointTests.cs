using FastEndpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Endpoint = PollingStationsResolver.Api.Features.ImportJob.DownloadTemplate.Endpoint;

namespace PollingStationsResolver.Api.Tests.Endpoints;

public class ImportJobDownloadTemplateEndpointTests
{
    private readonly Endpoint _endpoint;

    public ImportJobDownloadTemplateEndpointTests()
    {
        _endpoint = Factory.Create<Endpoint>();
    }

    [Fact]
    public async Task ReturnCorrectFile()
    {
        await _endpoint.HandleAsync(CancellationToken.None);

        _endpoint.ValidationFailed.Should().BeFalse();
        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        _endpoint.HttpContext.Response.ContentLength.Should().NotBe(0);
    }
}
