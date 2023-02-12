using PollingStationsResolver.Api.Features.Token.Create;
using FluentValidation.TestHelper;

namespace PollingStationsResolver.Api.Tests.Validators;

public class TokenCreateValidatorTests
{
    private readonly Validator _validator;

    public TokenCreateValidatorTests()
    {
        _validator = new Validator();
    }
    [Fact]
    public void Should_Have_Validation_Error_When_Username_Is_Empty()
    {
        var request = new Request { Username = "", Password = "password" };
        
        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Username);
    }

    [Fact]
    public void Should_Have_Validation_Error_When_Password_Is_Empty()
    {
        var request = new Request { Username = "username", Password = "" };
        
        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Password);

    }

    [Fact]
    public void Should_Not_Have_Validation_Error_When_Username_And_Password_Are_Not_Empty()
    {
        var request = new Request { Username = "username", Password = "password" };
        
        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
