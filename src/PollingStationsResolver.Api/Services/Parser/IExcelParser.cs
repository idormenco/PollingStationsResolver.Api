namespace PollingStationsResolver.Api.Services.Parser;

public interface IExcelParser
{
    ExcelParseResponse ParsePollingStations(IFormFile requestFile);
}