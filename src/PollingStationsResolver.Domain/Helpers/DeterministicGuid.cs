using System.Security.Cryptography;
using System.Text;

namespace PollingStationsResolver.Domain.Helpers;

public class DeterministicGuid
{
    public static Guid Create(string county, string locality, string address)
    {
        var key = $"{county}{locality}{address}";

        var hash = SHA256
            .Create()
            .ComputeHash(Encoding.UTF8.GetBytes(key));

        var guid = new Guid(hash.Take(16).ToArray());

        return guid;
    }
}
