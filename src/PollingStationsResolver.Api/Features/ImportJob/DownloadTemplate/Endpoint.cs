using System.Text;
namespace PollingStationsResolver.Api.Features.ImportJob.DownloadTemplate;


public class Endpoint : EndpointWithoutRequest
{
    private static readonly byte[] content = Encoding.UTF8.GetBytes("tbd");

    public override void Configure()
    {
        Get("/import-job/template");
        AllowAnonymous();
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        return SendBytesAsync(content, fileName: "import-template.xlsx");
    }
}
