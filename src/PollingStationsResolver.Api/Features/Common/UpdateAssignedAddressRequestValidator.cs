using FluentValidation;

namespace PollingStationsResolver.Api.Features.Common;

public class UpdateAssignedAddressRequestValidator : Validator<UpdateAssignedAddressRequest>
{
    public UpdateAssignedAddressRequestValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty).When(x => x.Id.HasValue);
        RuleFor(x => x.Locality).NotEmpty().MaximumLength(1024);
        RuleFor(x => x.StreetCode).NotEmpty().MaximumLength(1024);
        RuleFor(x => x.Street).NotEmpty().MaximumLength(1024);
        RuleFor(x => x.HouseNumbers).NotEmpty().MaximumLength(1024);
        RuleFor(x => x.Remarks).MaximumLength(1024);
    }
}
