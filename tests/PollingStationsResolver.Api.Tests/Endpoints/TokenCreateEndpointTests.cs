using FastEndpoints;
using FluentAssertions;
using NSubstitute;
using PollingStationsResolver.Api.Features.Token.Create;
using PollingStationsResolver.Api.Options;
using PollingStationsResolver.Api.Services.Credentials;
using PollingStationsResolver.Api.Tests.TestsHelpers;

namespace PollingStationsResolver.Api.Tests.Endpoints;

public class TokenCreateEndpointTests
{
    private readonly ICredentialsChecker _credentialsChecker;
    private readonly Endpoint _endpoint;

    public TokenCreateEndpointTests()
    {
        _credentialsChecker = Substitute.For<ICredentialsChecker>();
        var options = new TokenOptions
            {
                SigningKey = "SecretKeyOfDoomThatMustBeAMinimumNumberOfBytes"
            }
            .AsOptions();

        _endpoint = Factory.Create<Endpoint>(_credentialsChecker, options);
    }

    [Fact]
    public async Task CreatesToken_When_LoginSucceeds()
    {
        _credentialsChecker
            .CheckCredentials(Arg.Any<string>(), Arg.Any<string>())
            .Returns(true);

        var request = new Request()
        {
            Password = "password",
            Username = "test"
        };

        await _endpoint.HandleAsync(request, CancellationToken.None);

        _endpoint.ValidationFailed.Should().BeFalse();
        _endpoint.Response.Token.Should().StartWith("ey");
        _endpoint.Response.Expires.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task ValidationFails_When_LoginFails()
    {

        var request = new Request
        {
            Password = "password",
            Username = "test"
        };

        Func<Task> action = () => _endpoint.HandleAsync(request, CancellationToken.None);
        await action.Should().ThrowAsync<ValidationFailureException>();
        _endpoint.ValidationFailed.Should().BeTrue();
        _endpoint.ValidationFailures.Should().Contain(x => x.ErrorMessage == "The supplied credentials are invalid!");
    }
}
