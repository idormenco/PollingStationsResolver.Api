using FluentAssertions;
using PollingStationsResolver.Domain.Entities;

namespace PollingStationsResolver.Api.Tests.Aggregates;

public class ResolvedAddressTests
{
    [Fact]
    public void ResolvedAddress_Constructor_ShouldSetCorrectValues()
    {
        // Arrange
        var pollingStation = new ResolvedAddress("county", "locality","address",2,3);

        // Assert
        pollingStation.County.Should().Be("county");
        pollingStation.Locality.Should().Be("locality");
        pollingStation.Address.Should().Be("address");
        pollingStation.Latitude.Should().Be(2);
        pollingStation.Longitude.Should().Be(3);
    }
}
