﻿
using FluentValidation.TestHelper;
using PollingStationsResolver.Api.Features.Common;
using PollingStationsResolver.Api.Features.ImportedPollingStation.Update;
using PollingStationsResolver.Api.Tests.TestsHelpers;

namespace PollingStationsResolver.Api.Tests.Validators;

public class ImportedPollingStationUpdateValidatorTests
{
    private readonly Validator _validator;

    public ImportedPollingStationUpdateValidatorTests()
    {
        _validator = new Validator();
    }

    [Fact]
    public void Id_ShouldNotBeEmpty()
    {
        var request = new UpdateImportedPollingStationRequest() { Id = Guid.Empty };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Theory]
    [InlineData(-91)]
    [InlineData(91)]
    public void Latitude_ShouldBeWithinValidRange(double latitude)
    {
        var request = new UpdateImportedPollingStationRequest() { Latitude = latitude };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Latitude);
    }

    [Theory]
    [InlineData(-181)]
    [InlineData(181)]
    public void Longitude_ShouldBeWithinValidRange(double longitude)
    {
        var request = new UpdateImportedPollingStationRequest() { Longitude = longitude };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Longitude);
    }

    [Theory]
    [MemberData(nameof(InvalidStringsTestCases))]
    public void County_ShouldNotBeEmpty_And_ShouldNotExceedMaxLength(string county)
    {
        var request = new UpdateImportedPollingStationRequest() { County = county };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.County);
    }


    [Theory]
    [MemberData(nameof(InvalidStringsTestCases))]
    public void Locality_ShouldNotBeEmpty_And_ShouldNotExceedMaxLength(string locality)
    {
        var request = new UpdateImportedPollingStationRequest() { Locality = locality };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Locality);
    }

    [Theory]
    [MemberData(nameof(InvalidStringsTestCases))]
    public void PollingStationNumber_ShouldNotBeEmpty_And_ShouldNotExceedMaxLength(string pollingStationNumber)
    {
        var request = new UpdateImportedPollingStationRequest() { PollingStationNumber = pollingStationNumber };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.PollingStationNumber);
    }

    [Theory]
    [MemberData(nameof(InvalidStringsTestCases))]
    public void Address_ShouldNotBeEmpty_And_ShouldNotExceedMaxLength(string address)
    {
        var request = new UpdateImportedPollingStationRequest() { Address = address };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Address);
    }

    [Fact]
    public void AssignedAddresses_ShouldNotContainInvalidAddresses()
    {
        var request = new UpdateImportedPollingStationRequest
        {
            Locality = "locality",
            Address = "address",
            County = "county",
            Id = Guid.NewGuid(),
            Latitude = 78,
            Longitude = 69,
            PollingStationNumber = "123",
            AssignedAddresses = new UpdateAssignedAddressRequest[]
            {
                new()
                {
                    Locality = "",
                    StreetCode = "",
                    Street = "",
                    HouseNumbers = "",
                    Remarks = "s".Repeat(1025)
                },
                new()
                {
                    Locality = "Locality",
                    StreetCode = "StreetCode",
                    Street = "Street",
                    HouseNumbers = "123",
                    Remarks = "Remarks",
                }
            }
        };
        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor("AssignedAddresses[0].Locality");
        result.ShouldHaveValidationErrorFor("AssignedAddresses[0].StreetCode");
        result.ShouldHaveValidationErrorFor("AssignedAddresses[0].HouseNumbers");
        result.ShouldHaveValidationErrorFor("AssignedAddresses[0].Street");
    }

    public static IEnumerable<object?[]> InvalidStringsTestCases =>
        new List<object?[]>
        {
            new object?[] { null },
            new object[] { ""},
            new object[] { "\t"},
            new object[] { new string('c',1025)},
        };
}
