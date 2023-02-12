using FluentValidation;

namespace PollingStationsResolver.Api.Features.ImportedPollingStation.Delete;

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.JobId).NotEmpty();
        RuleFor(x => x.Id).NotEmpty();
    }
}
