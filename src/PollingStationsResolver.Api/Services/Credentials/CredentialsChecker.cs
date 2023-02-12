using Microsoft.Extensions.Options;
using PollingStationsResolver.Api.Options;

namespace PollingStationsResolver.Api.Services.Credentials;

public class CredentialsChecker : ICredentialsChecker
{
    private readonly ApplicationAdminCredentials _adminCredentials;

    public CredentialsChecker(IOptions<ApplicationAdminCredentials> adminCredentials)
    {
        _adminCredentials = adminCredentials.Value;
    }

    public bool CheckCredentials(string username, string password)
    {
        if (_adminCredentials.UserName == username && _adminCredentials.Password == password)
        {
            return true;
        }

        return false;
    }
}