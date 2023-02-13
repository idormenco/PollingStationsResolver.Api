using FluentAssertions;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.PollingStationAggregate;

namespace PollingStationsResolver.Api.Tests.Aggregates;

public class PollingStationTests
{
    [Fact]
    public void PollingStation_Constructor_ShouldSetCorrectValues()
    {
        // Arrange
        var pollingStation = new PollingStation("5678", "county", "locality", "address", 2, 3);

        // Assert
        pollingStation.PollingStationNumber.Should().Be("5678");
        pollingStation.County.Should().Be("county");
        pollingStation.Locality.Should().Be("locality");
        pollingStation.Address.Should().Be("address");
        pollingStation.Latitude.Should().Be(2);
        pollingStation.Longitude.Should().Be(3);
    }

    [Fact]
    public void AddAssignedAddress_AddsAddressToAssignedAddresses()
    {
        // Arrange
        var importedPollingStation = BobBuilder.CreatePollingStation();

        // Act
        importedPollingStation.AddAssignedAddress("locality", "code", "street", "numbers", "remarks");

        // Assert
        importedPollingStation.AssignedAddresses.Should().HaveCount(3);
        importedPollingStation.AssignedAddresses.Last().Locality.Should().Be("locality");
        importedPollingStation.AssignedAddresses.Last().StreetCode.Should().Be("code");
        importedPollingStation.AssignedAddresses.Last().Street.Should().Be("street");
        importedPollingStation.AssignedAddresses.Last().HouseNumbers.Should().Be("numbers");
        importedPollingStation.AssignedAddresses.Last().Remarks.Should().Be("remarks");
    }

    [Fact]
    public void UpdateDetails_UpdatesDetailsOfPollingStation()
    {
        // Arrange
        var importedPollingStation = BobBuilder.CreatePollingStation();

        // Act
        importedPollingStation.UpdateDetails("5678", "newCounty", "newLocality", "newAddress", 2, 3);

        // Assert
        importedPollingStation.PollingStationNumber.Should().Be("5678");
        importedPollingStation.County.Should().Be("newCounty");
        importedPollingStation.Locality.Should().Be("newLocality");
        importedPollingStation.Address.Should().Be("newAddress");
        importedPollingStation.Latitude.Should().Be(2);
        importedPollingStation.Longitude.Should().Be(3);
    }

    [Fact]
    public void DeleteAddress_DeletesAddress()
    {
        // Arrange
        var importedPollingStation = BobBuilder.CreatePollingStation();

        var deletedAddress = importedPollingStation.AssignedAddresses.First();

        // Act
        importedPollingStation.DeleteAddress(deletedAddress.Id);

        // Assert
        importedPollingStation.AssignedAddresses.Should().NotContain(deletedAddress);
    }

    [Fact]
    public void UpdateAddress_UpdatesAddressDetails()
    {
        // Arrange
        var importedPollingStation = BobBuilder.CreatePollingStation();

        var addressId = importedPollingStation.AssignedAddresses.First().Id;

        // Act
        importedPollingStation.UpdateAddress(addressId, "newLocality", "newCode", "newStreet", "newNumbers",
            "newRemarks");

        // Assert
        var address = importedPollingStation.AssignedAddresses.First();
        address.Should().NotBeNull();
        address!.Locality.Should().Be("newLocality");
        address!.StreetCode.Should().Be("newCode");
        address!.Street.Should().Be("newStreet");
        address!.HouseNumbers.Should().Be("newNumbers");
        address!.Remarks.Should().Be("newRemarks");
    }

    [Fact]
    public void DeleteAddress_ThrowsErrorIfAddressNotFound()
    {
        // Arrange
        var importedPollingStation = BobBuilder.CreatePollingStation();

        // Act
        Action action = () => importedPollingStation.DeleteAddress(Guid.NewGuid());

        // Assert
        action.Should().Throw<Exception>().WithMessage("Cannot find address with requested id. (Parameter 'id')"); ;
    }

    [Fact]
    public void UpdateAddress_ThrowsErrorIfAddressNotFound()
    {
        // Arrange
        var importedPollingStation = BobBuilder.CreatePollingStation();

        // Act
        Action action = () => importedPollingStation.UpdateAddress(Guid.NewGuid(), "newLocality", "newCode", "newStreet", "newNumbers",
             "newRemarks");

        // Assert
        action.Should().Throw<Exception>().WithMessage("Cannot find address with requested id. (Parameter 'id')");
    }
}
