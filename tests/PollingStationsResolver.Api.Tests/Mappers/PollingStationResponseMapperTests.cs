using FluentAssertions;
using PollingStationsResolver.Api.Features.PollingStation;
using PollingStationsResolver.Api.Tests.TestsHelpers;

namespace PollingStationsResolver.Api.Tests.Mappers;

public class PollingStationResponseMapperTests
{
    [Fact]
    public void FromEntity_ShouldMapPropertiesCorrectly()
    {
        // Arrange
        var entity = BobBuilder.CreatePollingStation();
        var mapper = new ResponseMapper();

        // Act
        var result = mapper.FromEntity(entity);

        // Assert
        result.Id.Should().Be(entity.Id);
        result.Latitude.Should().Be(entity.Latitude);
        result.Longitude.Should().Be(entity.Longitude);
        result.County.Should().Be(entity.County);
        result.Locality.Should().Be(entity.Locality);
        result.Address.Should().Be(entity.Address);
        result.PollingStationNumber.Should().Be(entity.PollingStationNumber);

        result.AssignedAddresses.Should().NotBeEmpty();
        var firstAssignedAddress = result.AssignedAddresses.First();
        firstAssignedAddress.Id.Should().Be(entity.AssignedAddresses.First().Id);
        firstAssignedAddress.StreetCode.Should().Be(entity.AssignedAddresses.First().StreetCode);
        firstAssignedAddress.Street.Should().Be(entity.AssignedAddresses.First().Street);
        firstAssignedAddress.Locality.Should().Be(entity.AssignedAddresses.First().Locality);
        firstAssignedAddress.HouseNumbers.Should().Be(entity.AssignedAddresses.First().HouseNumbers);
        firstAssignedAddress.Remarks.Should().Be(entity.AssignedAddresses.First().Remarks);
    }
}

