namespace PollingStationsResolver.Api.Features.Token.Create;


public record Request
{
    public string Username { get; init; }
    public string Password { get; init; }
}
