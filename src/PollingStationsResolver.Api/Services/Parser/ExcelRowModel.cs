namespace PollingStationsResolver.Api.Services.Parser;

public sealed record ExcelRowModel
{
    public string? County { get; init; }
    public string? PollingStationLocality { get; init; }
    public string? Code { get; init; }
    public string? OfficeNr { get; init; }
    public string? Institution { get; init; }
    public string? PollingStationNumber { get; init; }
    public string? Address { get; init; }
    public string? AssignedAddressLocality { get; init; }
    public string? StreetCode { get; init; }
    public string? Street { get; init; }
    public string? HouseNumbers { get; init; }
    public string? Remarks { get; init; }
}