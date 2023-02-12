using FluentValidation;

namespace PollingStationsResolver.Api.Features.Common;

public class AddAssignedAddressRequestValidator : Validator<AddAssignedAddressRequest>
{
    public AddAssignedAddressRequestValidator()
    {
        RuleFor(x => x.Locality).NotEmpty().MaximumLength(1024);
        RuleFor(x => x.StreetCode).NotEmpty().MaximumLength(1024);
        RuleFor(x => x.Street).NotEmpty().MaximumLength(1024);
        RuleFor(x => x.HouseNumbers).NotEmpty().MaximumLength(1024);
        RuleFor(x => x.Remarks).MaximumLength(1024);
    }
}
