using System.Collections.Immutable;
using System.Data;
using ExcelDataReader;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Api.Services.Parser;

public class ExcelParser : IExcelParser
{
    private readonly ILogger<ExcelParser> _logger;

    public ExcelParser(ILogger<ExcelParser> logger)
    {
        _logger = logger;
    }

    public ExcelParseResponse ParsePollingStations(IFormFile requestFile)
    {
        try
        {
            DataSet excelDataSet;

            using (var reader = ExcelReaderFactory.CreateReader(requestFile.OpenReadStream()))
            {
                excelDataSet = reader.AsDataSet();
            }

            var pollingStationsData = excelDataSet.Tables[0];

            var parsedRows = ImmutableList.CreateBuilder<ExcelRowModel>();

            int index = 1;
            string county = string.Empty;
            string pollingStationLocality = string.Empty;
            string code = string.Empty;
            string officeNr = string.Empty;
            string pollingStationNumber = string.Empty;
            string institution = string.Empty;
            string address = string.Empty;
            string locality = string.Empty;
            string streetCode = string.Empty;
            string street = string.Empty;
            string houseNumbers = string.Empty;
            string remarks = string.Empty;
            do
            {
                DataRow row = pollingStationsData.Rows[index];

                county = GetStringOrDefault(row[0], county);
                code = GetStringOrDefault(row[2], code);
                officeNr = GetStringOrDefault(row[3], officeNr);
                pollingStationNumber = GetStringOrDefault(row[4], pollingStationNumber);
                institution = GetStringOrDefault(row[5], institution);
                address = GetStringOrDefault(row[6], address);
                locality = GetStringOrDefault(row[8], locality);
                streetCode = GetStringOrDefault(pollingStationsData.Rows[index][9], streetCode);
                street = GetStringOrDefault(pollingStationsData.Rows[index][10], street);
                houseNumbers = GetStringOrDefault(pollingStationsData.Rows[index][11], houseNumbers);
                remarks = GetStringOrDefault(pollingStationsData.Rows[index][12], remarks);

                if (address.StartsWith("Loc. ", StringComparison.InvariantCultureIgnoreCase))
                {
                    address = address.Replace("Loc. ", "", StringComparison.InvariantCultureIgnoreCase);
                }

                pollingStationLocality = address.Split(",").FirstOrDefault();

                parsedRows.Add(new ExcelRowModel
                {
                    County = county,
                    PollingStationLocality = pollingStationLocality,
                    Code = code,
                    OfficeNr = officeNr,
                    PollingStationNumber = pollingStationNumber,
                    Institution = institution,
                    Address = address,
                    AssignedAddressLocality = locality,
                    StreetCode = streetCode,
                    Street = street,
                    HouseNumbers = houseNumbers,
                    Remarks = remarks,
                });

                ++index;

            } while (index < pollingStationsData.Rows.Count);

            var pollingStations = parsedRows
                .GroupBy(
                    r => new
                    {
                        r.County,
                        r.PollingStationNumber,
                        r.PollingStationLocality,
                        r.Institution,
                        r.Address
                    })
                .ToImmutableList();

            var validationMessagesBuilder = ImmutableList.CreateBuilder<string>();

            //foreach (var group in pollingStations)
            //{
            //    // todo: implement
            //    validationMessagesBuilder.Add("");
            //}

            if (validationMessagesBuilder.Any())
            {
                return new ExcelParseResponse.ValidationFailed(validationMessagesBuilder.ToImmutable());
            }

            var result = pollingStations
                  .Select(x => new ImportedPollingStation(x.Key.PollingStationNumber, x.Key.County, x.Key.PollingStationLocality, x.Key.Address, null, null, ResolvedAddressStatus.NotProcessed))
                  .ToImmutableList();


            return new ExcelParseResponse.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error when parsing excel file");
            return new ExcelParseResponse.Error("Error parsing excel file");
        }
    }

    private static string GetStringOrDefault(object? value, string defaultValue)
    {
        if (value == null)
        {
            return defaultValue;
        }

        var text = value.ToString();

        if (string.IsNullOrWhiteSpace(text))
        {
            return defaultValue;
        }

        return text.Trim();
    }


}