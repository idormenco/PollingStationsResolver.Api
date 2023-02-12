using System.Text;
using Microsoft.Extensions.Options;

namespace PollingStationsResolver.Api.Tests.TestsHelpers;

public static class Utils
{
    public static string Repeat(this string seed, int times)
    {
        var result = new StringBuilder();
        for (var i = 0; i < times; i++)
        {
            result.Append(seed);
        }
        return result.ToString();
    }

    public static IOptions<T> AsOptions<T>(this T value) where T : class
    {
        return Microsoft.Extensions.Options.Options.Create(value);
    }

    public static string ReadBody(this Stream body)
    {
        using var memoryStream = new MemoryStream();
        body.CopyTo(memoryStream);
        return Convert.ToBase64String(memoryStream.ToArray());
    }
}
