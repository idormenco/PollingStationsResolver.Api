using System.Collections.Immutable;
using PollingStationEntity = PollingStationsResolver.Domain.Entities.PollingStationAggregate.PollingStation;

namespace PollingStationsResolver.Api.Features.PollingStation;

public class ResponseMapper : ResponseMapper<PollingStationModel, PollingStationEntity>
{
    public override PollingStationModel FromEntity(PollingStationEntity entity)
    {
        return new PollingStationModel
        {
            Id = entity.Id,
            Latitude = entity.Latitude,
            Longitude = entity.Longitude,
            County = entity.County,
            Locality = entity.Locality,
            Address = entity.Address,
            PollingStationNumber = entity.PollingStationNumber,
            AssignedAddresses = entity.AssignedAddresses.Select(x => new PollingStationModel.AddressModel()
            {
                Id = x.Id,
                StreetCode = x.StreetCode,
                Street = x.Street,
                Locality = x.Locality,
                HouseNumbers = x.HouseNumbers,
                Remarks = x.Remarks,
            }).ToImmutableArray()
        };
    }
}
