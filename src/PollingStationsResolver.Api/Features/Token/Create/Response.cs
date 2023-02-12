namespace PollingStationsResolver.Api.Features.Token.Create;

public record Response
{
    public DateTime Expires { get; init; }
    public string Token { get; init; }
}
