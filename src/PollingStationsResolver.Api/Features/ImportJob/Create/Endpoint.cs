using PollingStationsResolver.Api.Services.Parser;
using PollingStationsResolver.Domain.Repository;
using ImportJobEntity = PollingStationsResolver.Domain.Entities.ImportJobAggregate.ImportJob;
using ImportedPollingStationEntity = PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate.ImportedPollingStation;

namespace PollingStationsResolver.Api.Features.ImportJob.Create;

public class Endpoint : Endpoint<ImportRequest, ImportJobModel, ResponseMapper>
{
    private readonly IRepository<ImportJobEntity> _repository;
    private readonly IRepository<ImportedPollingStationEntity> _importedPollingStationsRepository;
    private readonly IExcelParser _excelParser;

    public Endpoint(IRepository<ImportJobEntity> repository, IRepository<ImportedPollingStationEntity> importedPollingStationsRepository, IExcelParser excelParser)
    {
        _repository = repository;
        _importedPollingStationsRepository = importedPollingStationsRepository;
        _excelParser = excelParser;
    }

    public override void Configure()
    {
        Post("/import-job");
        AllowFileUploads();
        AllowAnonymous();
    }

    public override async Task HandleAsync(ImportRequest request, CancellationToken ct)
    {
        var parseResult = _excelParser.ParsePollingStations(request.File);

        switch (parseResult)
        {
            case ExcelParseResponse.Success(var pollingStations):
                var base64File = EncodeFile(request.File);
                var importJob = new ImportJobEntity(request.File.FileName, base64File);
                importJob = await _repository.AddAsync(importJob, ct);
                foreach (var importedPollingStation in pollingStations)
                {
                    importedPollingStation.AssignToJob(importJob.Id);
                }
                await _importedPollingStationsRepository.AddRangeAsync(pollingStations, ct);

                await SendOkAsync(Map.FromEntity(importJob), ct);
                break;

            case ExcelParseResponse.Error(var message):
                AddError(message);
                await SendErrorsAsync(cancellation: ct);
                break;

            case ExcelParseResponse.ValidationFailed(var messages):
                foreach (var message in messages)
                {
                    AddError(message);
                }
                await SendErrorsAsync(cancellation: ct);
                break;

            default:
                ThrowError("Unhandled parse result");
                break;

        }
    }

    private static string EncodeFile(IFormFile file)
    {
        using var reader = new BinaryReader(file!.OpenReadStream());
        string base64File = Convert.ToBase64String(reader.ReadBytes((int)file.Length));
        return base64File;
    }
}
