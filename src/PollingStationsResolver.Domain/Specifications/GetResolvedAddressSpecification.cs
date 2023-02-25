using Ardalis.Specification;
using PollingStationsResolver.Domain.Entities;
using PollingStationsResolver.Domain.Helpers;

namespace PollingStationsResolver.Domain.Specifications;

public class GetResolvedAddressSpecification: Specification<ResolvedAddress>, ISingleResultSpecification<ResolvedAddress>
{
    public GetResolvedAddressSpecification(string county, string locality, string address)
    {
        var addressId = DeterministicGuid.Create(county, locality, address);

        Query.Where(x => x.Id == addressId);
    }
}
