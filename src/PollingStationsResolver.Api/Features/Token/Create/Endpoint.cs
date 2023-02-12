using Microsoft.Extensions.Options;
using PollingStationsResolver.Api.Options;
using PollingStationsResolver.Api.Services.Credentials;

namespace PollingStationsResolver.Api.Features.Token.Create;

public  class Endpoint : Endpoint<Request, Response>
{
    private readonly ICredentialsChecker _credentialsChecker;
    private readonly TokenOptions _options;

    public Endpoint(ICredentialsChecker credentialsChecker, IOptions<TokenOptions> options)
    {
        _credentialsChecker = credentialsChecker;
        _options = options.Value;
    }

    public override void Configure()
    {
        Post("token");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        var credentialsValid = _credentialsChecker.CheckCredentials(request.Username, request.Password);
        if (credentialsValid)
        {
            var expireAt = DateTime.UtcNow.AddDays(1);
            var token = JWTBearer.CreateToken(
                signingKey: _options.SigningKey,
                expireAt: expireAt,
                priviledges: u =>
                {
                    u.Roles.Add("Admin");
                    u.Claims.Add(new("Username", request.Username));
                });

            var response = new Response()
            {
                Expires = expireAt,
                Token = token
            };

            await SendAsync(response, cancellation: ct);
        }
        else
        {
            ThrowError("The supplied credentials are invalid!");
        }
    }
}
