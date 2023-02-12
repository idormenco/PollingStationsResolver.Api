using Ardalis.SmartEnum;

namespace PollingStationsResolver.Domain.Entities.ImportJobAggregate;

public sealed class ResolvedAddressStatus : SmartEnum<ResolvedAddressStatus>
{
    public static readonly ResolvedAddressStatus NotProcessed = new(nameof(NotProcessed), 1);
    public static readonly ResolvedAddressStatus Success = new(nameof(Success), 2);
    public static readonly ResolvedAddressStatus NotFound = new(nameof(NotFound), 3);

    private ResolvedAddressStatus(string name, int value) : base(name, value)
    {
    }
}
