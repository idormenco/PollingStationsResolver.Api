namespace PollingStationsResolver.Api.Services.Credentials;

public interface ICredentialsChecker
{
    bool CheckCredentials(string username, string password);
}