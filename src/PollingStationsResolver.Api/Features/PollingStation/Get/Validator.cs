using FluentValidation;

namespace PollingStationsResolver.Api.Features.PollingStation.Get;

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
