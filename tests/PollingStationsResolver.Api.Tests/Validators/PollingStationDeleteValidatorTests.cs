using FluentValidation.TestHelper;
using PollingStationsResolver.Api.Features.PollingStation.Delete;

namespace PollingStationsResolver.Api.Tests.Validators;

public class PollingStationDeleteValidatorTests
{
    private readonly Validator _validator;

    public PollingStationDeleteValidatorTests()
    {
        _validator = new Validator();
    }

    [Fact]
    public void Id_ShouldNotBeEmpty()
    {
        var request = new Request() { Id = Guid.Empty };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}
