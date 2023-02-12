using FluentValidation;

namespace PollingStationsResolver.Api.Features.PollingStation.Delete;

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
