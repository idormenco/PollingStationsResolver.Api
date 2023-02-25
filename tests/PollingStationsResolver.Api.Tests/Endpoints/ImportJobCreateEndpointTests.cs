using System.Collections.Immutable;
using System.Text;
using FastEndpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PollingStationsResolver.Api.Features.ImportJob;
using PollingStationsResolver.Api.Features.ImportJob.Create;
using PollingStationsResolver.Api.Services.Parser;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;
using PollingStationsResolver.Domain.Repository;
using PollingStationsResolver.Domain.Specifications;
using Endpoint = PollingStationsResolver.Api.Features.ImportJob.Create.Endpoint;

namespace PollingStationsResolver.Api.Tests.Endpoints;

public class ImportJobCreateEndpointTests
{
    private readonly IRepository<ImportJob> _repository;
    private readonly IRepository<ImportedPollingStation> _importedPollingStationsRepository;
    private readonly IExcelParser _excelParser;
    private readonly Endpoint _endpoint;

    public ImportJobCreateEndpointTests()
    {
        _repository = Substitute.For<IRepository<ImportJob>>();
        _importedPollingStationsRepository = Substitute.For<IRepository<ImportedPollingStation>>();
        _excelParser = Substitute.For<IExcelParser>();

        _endpoint = Factory.Create<Endpoint>(_repository, _importedPollingStationsRepository, _excelParser);
        _endpoint.Map = new ResponseMapper();
    }

    [Fact]
    public async Task HandleAsync_WithValidRequestAndNoImportJobInProgress_ReturnsOkResponse()
    {
        // Arrange
        var parsedPollingStations = ImmutableArray.Create(BobBuilder.CreateImportedPollingStation(),
            BobBuilder.CreateImportedPollingStation(),
            BobBuilder.CreateImportedPollingStation());

        _excelParser
            .ParsePollingStations(Arg.Any<IFormFile>())
            .Returns(new ExcelParseResponse.Success(parsedPollingStations));

        var importJob = BobBuilder.CreateImportJob();
        _repository.AddAsync(Arg.Any<ImportJob>()).Returns(importJob);

        // Act
        await _endpoint.HandleAsync(new ImportRequest() { File = CreateDummyExcelFile() }, CancellationToken.None);

        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        _endpoint.ValidationFailed.Should().BeFalse();
        _endpoint.Response.Should().NotBeNull();
        _endpoint.Response.Should().BeEquivalentTo(importJob, config => config.Excluding(x => x.File));
    }

    [Fact]
    public async Task HandleAsync_WithValidRequestAndNoImportJobInProgress_CreatesJob()
    {
        // Arrange
        _excelParser
            .ParsePollingStations(Arg.Any<IFormFile>())
            .Returns(new ExcelParseResponse.Success(ImmutableArray<ImportedPollingStation>.Empty));

        var importJob = BobBuilder.CreateImportJob();
        _repository
            .AddAsync(Arg.Any<ImportJob>())
            .Returns(importJob);

        // Act
        await _endpoint.HandleAsync(new ImportRequest() { File = CreateDummyExcelFile() }, CancellationToken.None);

        // Assert
        await _repository
            .Received(1)
            .AddAsync(Arg.Is<ImportJob>(x => x.FileName == "dummy.xlsx"
                                             && x.File.Base64File == "VGhpcyBpcyBhIGR1bW15IGZpbGU="
                                             && x.JobStatus == ImportJobStatus.NotStarted));
    }

    [Fact]
    public async Task HandleAsync_WithValidRequestAndNoImportJobInProgress_CreatesImportedPollingStation()
    {
        // Arrange
        var parsedPollingStations = ImmutableArray.Create(BobBuilder.CreateImportedPollingStation(),
            BobBuilder.CreateImportedPollingStation(),
            BobBuilder.CreateImportedPollingStation());

        _excelParser
            .ParsePollingStations(Arg.Any<IFormFile>())
            .Returns(new ExcelParseResponse.Success(parsedPollingStations));

        var importJob = BobBuilder.CreateImportJob();
        _repository
            .AddAsync(Arg.Any<ImportJob>())
            .Returns(importJob);

        // Act
        await _endpoint.HandleAsync(new ImportRequest { File = CreateDummyExcelFile() }, CancellationToken.None);

        // Assert
        await _importedPollingStationsRepository
            .Received(1)
            .AddRangeAsync(Arg.Is<IEnumerable<ImportedPollingStation>>(x => x.Count() == 3 && x.All(ips => ips.JobId == importJob.Id)));
    }

    [Fact]
    public async Task HandleAsync_WithValidRequestAndImportJobInProgress_ThrowsError()
    {
        // Arrange
        _repository
            .AnyAsync(Arg.Any<CurrentImportJobInProgressSpecification>())
            .Returns(true);

        // Act
        Func<Task> action = () => _endpoint.HandleAsync(new ImportRequest { File = CreateDummyExcelFile() }, CancellationToken.None);
        await action.Should().ThrowAsync<ValidationFailureException>();

        // Assert
        _endpoint.ValidationFailed.Should().BeTrue();
        _endpoint.ValidationFailures.Should().Contain(x => x.ErrorMessage == "Cannot import another file when it is an import job in progress.");

    }

    [Fact]
    public async Task HandleAsync_WithInvalidFileFormat_ThrowsError()
    {
        // Arrange
        _excelParser
            .ParsePollingStations(Arg.Any<IFormFile>())
            .Returns(new ExcelParseResponse.Error("Something bad happened"));

        // Act
        await _endpoint.HandleAsync(new ImportRequest { File = CreateDummyExcelFile() }, CancellationToken.None);

        // Assert
        _endpoint.ValidationFailed.Should().BeTrue();
        _endpoint.ValidationFailures.Select(x => x.ToString()).Should().BeEquivalentTo(new[] { "Something bad happened" });
    }

    [Fact]
    public async Task HandleAsync_WithValidRequestAndInvalidPollingStations_ThrowsError()
    {
        // Arrange
        var errors = ImmutableArray.Create("error message 1",
            "error message 2",
            "error message 3");

        _excelParser
            .ParsePollingStations(Arg.Any<IFormFile>())
            .Returns(new ExcelParseResponse.ValidationFailed(errors));

        // Act
        await _endpoint.HandleAsync(new ImportRequest { File = CreateDummyExcelFile() }, CancellationToken.None);

        // Assert
        _endpoint.ValidationFailed.Should().BeTrue();
        _endpoint.ValidationFailures.Select(x => x.ToString()).Should().BeEquivalentTo(errors);
    }

    private IFormFile CreateDummyExcelFile()
    {
        var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
        IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.xlsx");
        return file;
    }
}
