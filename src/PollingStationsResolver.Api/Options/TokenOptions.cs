namespace PollingStationsResolver.Api.Options;

public class TokenOptions
{
    public const string SectionKey = "Token";

    public string SigningKey { get; init; }
}
