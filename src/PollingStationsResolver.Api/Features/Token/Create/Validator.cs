using FluentValidation;

namespace PollingStationsResolver.Api.Features.Token.Create;

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Username)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
