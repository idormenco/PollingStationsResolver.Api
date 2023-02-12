
using FluentValidation.TestHelper;
using PollingStationsResolver.Api.Features.PollingStation.Get;

namespace PollingStationsResolver.Api.Tests.Validators;

public class PollingStationGetValidatorTests
{
    private readonly Validator _validator;

    public PollingStationGetValidatorTests()
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
