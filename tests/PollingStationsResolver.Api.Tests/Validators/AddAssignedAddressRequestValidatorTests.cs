using FluentValidation.TestHelper;
using PollingStationsResolver.Api.Features.Common;
using PollingStationsResolver.Api.Tests.TestsHelpers;

namespace PollingStationsResolver.Api.Tests.Validators;

public class AddAssignedAddressRequestValidatorTests
{
    private readonly AddAssignedAddressRequestValidator _validator;

    public AddAssignedAddressRequestValidatorTests()
    {
        _validator = new AddAssignedAddressRequestValidator();
    }

    [Theory]
    [MemberData(nameof(InvalidStringsTestCases))]
    public void Locality_ShouldNotBeEmpty_And_ShouldNotExceedMaxLength(string locality)
    {
        var request = new AddAssignedAddressRequest { Locality = locality };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Locality);
    }

    [Theory]
    [MemberData(nameof(InvalidStringsTestCases))]
    public void StreetCode_ShouldNotBeEmpty_And_ShouldNotExceedMaxLength(string streetCode)
    {
        var request = new AddAssignedAddressRequest { StreetCode = streetCode };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.StreetCode);
    }

    [Theory]
    [MemberData(nameof(InvalidStringsTestCases))]
    public void Street_ShouldNotBeEmpty_And_ShouldNotExceedMaxLength(string street)
    {
        var request = new AddAssignedAddressRequest { Street = street };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Street);
    }

    [Theory]
    [MemberData(nameof(InvalidStringsTestCases))]
    public void HouseNumbers_ShouldNotBeEmpty_And_ShouldNotExceedMaxLength(string houseNumbers)
    {
        var request = new AddAssignedAddressRequest { HouseNumbers = houseNumbers };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.HouseNumbers);
    }

    [Fact]
    public void Remarks_ShouldNotExceedMaxLength()
    {
        var request = new AddAssignedAddressRequest { Remarks = "s".Repeat(1025) };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Remarks);
    }

    public static IEnumerable<object[]> InvalidStringsTestCases =>
        new List<object[]>
        {
            new object[] { null},
            new object[] { ""},
            new object[] { "\t"},
            new object[] { "s".Repeat(1025)},
        };
}
