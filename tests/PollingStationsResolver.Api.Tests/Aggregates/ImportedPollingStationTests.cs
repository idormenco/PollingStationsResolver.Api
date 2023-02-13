using FluentAssertions;
using PollingStationsResolver.Api.Tests.TestsHelpers;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Api.Tests.Aggregates;

public class ImportedPollingStationTests
{
    [Fact]
    public void AddAssignedAddress_AddsAddressToAssignedAddresses()
    {
        // Arrange
        var importedPollingStation = BobBuilder.CreateImportedPollingStation();

        // Act
        importedPollingStation.AddAssignedAddress("locality", "code", "street", "numbers", "remarks");

        // Assert
        importedPollingStation.AssignedAddresses.Count().Should().Be(3);
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
        var importedPollingStation = BobBuilder.CreateImportedPollingStation();

        // Act
        importedPollingStation.UpdateDetails("5678", "newCounty", "newLocality", "newAddress", 2, 3,
            ResolvedAddressStatus.Success);

        // Assert
        importedPollingStation.PollingStationNumber.Should().Be("5678");
        importedPollingStation.County.Should().Be("newCounty");
        importedPollingStation.Locality.Should().Be("newLocality");
        importedPollingStation.Address.Should().Be("newAddress");
        importedPollingStation.Latitude.Should().Be(2);
        importedPollingStation.Longitude.Should().Be(3);
        importedPollingStation.ResolvedAddressStatus.Should().Be(ResolvedAddressStatus.Success);
    }

    [Fact]
    public void DeleteAddress_DeletesAddress()
    {
        // Arrange
        var importedPollingStation = BobBuilder.CreateImportedPollingStation();

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
        var importedPollingStation = BobBuilder.CreateImportedPollingStation();

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
        var importedPollingStation = BobBuilder.CreateImportedPollingStation();

        // Act
        Action action = () => importedPollingStation.DeleteAddress(Guid.NewGuid());

        // Assert
        action.Should().Throw<Exception>().WithMessage("Cannot find address with requested id. (Parameter 'id')"); ;
    }

    [Fact]
    public void UpdateAddress_ThrowsErrorIfAddressNotFound()
    {
        // Arrange
        var importedPollingStation = BobBuilder.CreateImportedPollingStation();

        // Act
        Action action = () => importedPollingStation.UpdateAddress(Guid.NewGuid(), "newLocality", "newCode", "newStreet", "newNumbers",
             "newRemarks");

        // Assert
        action.Should().Throw<Exception>().WithMessage("Cannot find address with requested id. (Parameter 'id')");
    }

    [Fact]
    public void UpdateCoordinates_ShouldUpdateLatitudeAndLongitude_WhenBothValuesAreNotNull()
    {
        // Arrange
        var importedPollingStation = BobBuilder.CreateImportedPollingStation();

        var latitude = 1.0;
        var longitude = 2.0;

        // Act
        importedPollingStation.UpdateCoordinates(latitude, longitude);

        // Assert
        importedPollingStation.Latitude.Should().Be(latitude);
        importedPollingStation.Longitude.Should().Be(longitude);
        importedPollingStation.ResolvedAddressStatus.Should().Be(ResolvedAddressStatus.Success);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(null, 1.0)]
    [InlineData(1.0, null)]
    public void UpdateCoordinates_ShouldThrowException_WhenLatitudeOrLongitudeIsNull(double? latitude, double? longitude)
    {
        // Arrange
        var importedPollingStation = BobBuilder.CreateImportedPollingStation();

        // Act
        Action act = () => importedPollingStation.UpdateCoordinates(latitude, longitude);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter '*')");
    }

    [Fact]
    public void AssignToJob_ShouldSetJobId()
    {
        // Arrange
        var importedPollingStation = BobBuilder.CreateImportedPollingStation();
        var jobId = Guid.NewGuid();

        // Act
        importedPollingStation.AssignToJob(jobId);

        // Assert
        importedPollingStation.JobId.Should().Be(jobId);
    }

    [Fact]
    public void AssignToJob_WithEmptyJobId_ShouldThrowException()
    {
        // Arrange
        var importedPollingStation = BobBuilder.CreateImportedPollingStation();

        Guid emptyJobId = Guid.Empty;

        // Act
        Action act = () => importedPollingStation.AssignToJob(emptyJobId);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Required input jobId was empty. (Parameter 'jobId')");
    }
}
