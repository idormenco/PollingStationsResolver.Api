using System.Text.Json;
using FluentAssertions;
using PollingStationsResolver.Geocoding.Nominatim;

namespace PollingStationsResolver.Geocoding.Tests;

public class ConverterTests
{
    [Fact]
    public void ShouldParseCorrectly()
    {
        var json = @"[
            {""place_id"":112274,""licence"":""Data © OpenStreetMap contributors, ODbL 1.0. https://osm.org/copyright"",""osm_type"":""node"",""osm_id"":462234257,""boundingbox"":[""47.1716115"",""47.2116115"",""27.1591611"",""27.1991611""],""lat"":""47.1916115"",""lon"":""27.1791611"",""display_name"":""Zmeu, Lungani, Iași, 707288, România"",""place_rank"":19,""category"":""place"",""type"":""village"",""importance"":0.47501},
            {""place_id"":1126143,""licence"":""Data © OpenStreetMap contributors, ODbL 1.0. https://osm.org/copyright"",""osm_type"":""way"",""osm_id"":670413927,""boundingbox"":[""47.1935296"",""47.1945557"",""27.1727406"",""27.1730212""],""lat"":""47.1935296"",""lon"":""27.1730212"",""display_name"":""Cotârgaci-Zmeu, Zmeu, Lungani, Iași, 707288, România"",""place_rank"":26,""category"":""highway"",""type"":""residential"",""importance"":0.30000999999999994},
            {""place_id"":827029,""licence"":""Data © OpenStreetMap contributors, ODbL 1.0. https://osm.org/copyright"",""osm_type"":""way"",""osm_id"":227684965,""boundingbox"":[""47.1945557"",""47.196968"",""27.1721491"",""27.1727406""],""lat"":""47.1960863"",""lon"":""27.1722874"",""display_name"":""Cotârgaci-Zmeu, Zmeu, Lungani, Iași, 707288, România"",""place_rank"":26,""category"":""highway"",""type"":""track"",""importance"":0.30000999999999994},
            {""place_id"":617464,""licence"":""Data © OpenStreetMap contributors, ODbL 1.0. https://osm.org/copyright"",""osm_type"":""way"",""osm_id"":26466930,""boundingbox"":[""47.1589415"",""47.1603128"",""27.5955338"",""27.5955511""],""lat"":""47.1599432"",""lon"":""27.5955511"",""display_name"":""Strada Zmeu, Centru, Iași, Zona Metropolitană Iași, Iași, 700400, România"",""place_rank"":26,""category"":""highway"",""type"":""residential"",""importance"":0.30000999999999994},
            {""place_id"":900794,""licence"":""Data © OpenStreetMap contributors, ODbL 1.0. https://osm.org/copyright"",""osm_type"":""way"",""osm_id"":273671353,""boundingbox"":[""47.2182473"",""47.2194187"",""27.1651205"",""27.1668667""],""lat"":""47.2186336"",""lon"":""27.1661183"",""display_name"":""Cotârgaci-Zmeu, Cotârgaci, Bălțați, Iași, 707026, România"",""place_rank"":26,""category"":""highway"",""type"":""unclassified"",""importance"":0.30000999999999994},
            {""place_id"":931250,""licence"":""Data © OpenStreetMap contributors, ODbL 1.0. https://osm.org/copyright"",""osm_type"":""way"",""osm_id"":273671351,""boundingbox"":[""47.2181557"",""47.2182473"",""27.1648963"",""27.1651205""],""lat"":""47.2181557"",""lon"":""27.1648963"",""display_name"":""Cotârgaci-Zmeu, Cotârgaci, Bălțați, Iași, 707026, România"",""place_rank"":26,""category"":""highway"",""type"":""track"",""importance"":0.30000999999999994}
        ]";

        var options = new JsonSerializerOptions
        {
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
        };
        var searchResults = JsonSerializer.Deserialize<SearchResult[]>(json, options);

        searchResults.Should().HaveCount(6);
        searchResults.First().Lat.Should().Be(47.1916115);
        searchResults.First().Lon.Should().Be(27.1791611);

    }
}
