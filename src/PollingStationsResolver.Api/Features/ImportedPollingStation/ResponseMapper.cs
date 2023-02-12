using System.Collections.Immutable;
using ImportedPollingStationEntity = PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate.ImportedPollingStation;
namespace PollingStationsResolver.Api.Features.ImportedPollingStation;

public class ResponseMapper : ResponseMapper<ImportedPollingStationModel, ImportedPollingStationEntity>
{
    public override ImportedPollingStationModel FromEntity(ImportedPollingStationEntity entity)
    {
        return new ImportedPollingStationModel
        {
            Id = entity.Id,
            JobId = entity.JobId,
            Latitude = entity.Latitude,
            Longitude = entity.Longitude,
            Locality = entity.Locality,
            Address = entity.Address,
            PollingStationNumber = entity.PollingStationNumber,
            County = entity.County,
            ResolvedAddressStatus = entity.ResolvedAddressStatus,
            AssignedAddresses = entity.AssignedAddresses.Select(x => new ImportedPollingStationModel.AddressModel()
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
