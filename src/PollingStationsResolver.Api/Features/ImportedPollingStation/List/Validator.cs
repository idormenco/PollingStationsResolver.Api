using FluentValidation;

namespace PollingStationsResolver.Api.Features.ImportedPollingStation.List;

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.JobId).NotEmpty();

        RuleFor(x => x.PageSize)
            .InclusiveBetween(10, 100);

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);
    }
}
