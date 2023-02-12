using Ardalis.SmartEnum;

namespace PollingStationsResolver.Domain.Entities.ImportJobAggregate;
public sealed class ImportJobStatus : SmartEnum<ImportJobStatus>
{

    public static readonly ImportJobStatus NotStarted = new(nameof(NotStarted), 1);
    public static readonly ImportJobStatus Started = new(nameof(Started), 2);
    public static readonly ImportJobStatus Finished = new(nameof(Finished), 3);
    public static readonly ImportJobStatus Canceled = new(nameof(Canceled), 4);
    public static readonly ImportJobStatus Imported = new(nameof(Imported), 5);

    private ImportJobStatus(string name, int value) : base(name, value)
    {
    }
}