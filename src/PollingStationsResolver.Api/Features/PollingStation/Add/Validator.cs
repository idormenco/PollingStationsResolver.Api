using FluentValidation;
using PollingStationsResolver.Api.Features.Common;

namespace PollingStationsResolver.Api.Features.PollingStation.Add;

public class Validator : Validator<AddPollingStationRequest>
{
    public Validator()
    {
        RuleFor(x => x.Latitude).NotEmpty().GreaterThanOrEqualTo(-90).LessThanOrEqualTo(90);
        RuleFor(x => x.Longitude).NotEmpty().GreaterThanOrEqualTo(-180).LessThanOrEqualTo(180);
        RuleFor(x => x.County).NotEmpty().MaximumLength(1024);
        RuleFor(x => x.Locality).NotEmpty().MaximumLength(1024);
        RuleFor(x => x.PollingStationNumber).NotEmpty().MaximumLength(1024);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(1024);

        RuleForEach(x => x.AssignedAddresses).SetValidator(new AddAssignedAddressRequestValidator());
    }
}
