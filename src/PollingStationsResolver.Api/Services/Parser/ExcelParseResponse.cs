using System.Collections.Immutable;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;

namespace PollingStationsResolver.Api.Services.Parser;

public abstract record ExcelParseResponse
{
    public record Error(string Message) : ExcelParseResponse;
    public record ValidationFailed(IImmutableList<string> Messages) : ExcelParseResponse;
    public record Success(IImmutableList<ImportedPollingStation> PollingStations) : ExcelParseResponse;

    private ExcelParseResponse(){}
}