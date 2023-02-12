using FluentValidation.TestHelper;
using PollingStationsResolver.Api.Features.Common;
using PollingStationsResolver.Api.Tests.TestsHelpers;

namespace PollingStationsResolver.Api.Tests.Validators;

public class UpdateAssignedAddressRequestValidatorTests
{
    private readonly UpdateAssignedAddressRequestValidator _validator;

    public UpdateAssignedAddressRequestValidatorTests()
    {
        _validator = new UpdateAssignedAddressRequestValidator();
    }

    [Fact]
    public void Id_ShouldNotBeEmpty_When_UpdatingExistingAddress()
    {
        var request = new UpdateAssignedAddressRequest() { Id = Guid.Empty };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Id_CanBeNull_When_AddingAddress()
    {
        var request = new UpdateAssignedAddressRequest() { Id = null };
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Theory]
    [MemberData(nameof(InvalidStringsTestCases))]
    public void Locality_ShouldNotBeEmpty_And_ShouldNotExceedMaxLength(string locality)
    {
        var request = new UpdateAssignedAddressRequest { Locality = locality };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Locality);
    }

    [Theory]
    [MemberData(nameof(InvalidStringsTestCases))]
    public void StreetCode_ShouldNotBeEmpty_And_ShouldNotExceedMaxLength(string streetCode)
    {
        var request = new UpdateAssignedAddressRequest { StreetCode = streetCode };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.StreetCode);
    }

    [Theory]
    [MemberData(nameof(InvalidStringsTestCases))]
    public void Street_ShouldNotBeEmpty_And_ShouldNotExceedMaxLength(string street)
    {
        var request = new UpdateAssignedAddressRequest { Street = street };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Street);
    }

    [Theory]
    [MemberData(nameof(InvalidStringsTestCases))]
    public void HouseNumbers_ShouldNotBeEmpty_And_ShouldNotExceedMaxLength(string houseNumbers)
    {
        var request = new UpdateAssignedAddressRequest { HouseNumbers = houseNumbers };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.HouseNumbers);
    }

    [Fact]
    public void Remarks_ShouldNotExceedMaxLength()
    {
        var request = new UpdateAssignedAddressRequest { Remarks = "s".Repeat(1025) };
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
