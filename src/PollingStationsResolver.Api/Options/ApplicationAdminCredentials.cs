namespace PollingStationsResolver.Api.Options;

public class ApplicationAdminCredentials
{
    public const string SectionKey = "AdminCredentials";
    public string UserName { get; init; }
    public string Password { get; init; }
}