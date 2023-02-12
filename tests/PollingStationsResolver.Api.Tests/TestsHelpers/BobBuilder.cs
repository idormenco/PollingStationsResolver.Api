using PollingStationsResolver.Api.Features.Common;
using PollingStationsResolver.Api.Features.ImportedPollingStation.Add;
using PollingStationsResolver.Api.Features.ImportedPollingStation.Update;
using PollingStationsResolver.Api.Features.PollingStation.Add;
using PollingStationsResolver.Api.Features.PollingStation.Update;
using PollingStationsResolver.Api.Tests.TestsHelpers.Fakers;
using PollingStationsResolver.Domain.Entities.ImportedPollingStationAggregate;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;
using PollingStationsResolver.Domain.Entities.PollingStationAggregate;

namespace PollingStationsResolver.Api.Tests.TestsHelpers;

public class BobBuilder
{
    public static ImportedPollingStation CreateImportedPollingStation(Guid? jobId = null, Guid? id = null, ResolvedAddressStatus? status = null)
    {
        var importedPollingStation = new ImportedPollingStationFaker(jobId ?? Guid.NewGuid(), id ?? Guid.NewGuid(), status: status).Generate();
        return importedPollingStation;
    }

    public static ImportJob CreateImportJob(Guid? id = null, ImportJobStatus? status = null)
    {
        var importJob = new ImportJobFaker(id ?? Guid.NewGuid(), status).Generate();

        return importJob;
    }

    public static PollingStation CreatePollingStation(Guid? id = null)
    {

        var pollingStation = new PollingStationFaker(id ?? Guid.NewGuid()).Generate();

        return pollingStation;
    }

    public static AddPollingStationRequest CreateAddPollingStationRequest()
    {
        var addPollingStationRequest = new AddPollingStationRequestFaker().Generate();

        return addPollingStationRequest;
    }

    public static AddImportedPollingStationRequest CreateAddImportedPollingStationRequest(Guid jobId)
    {
        var addImportedPollingStationRequest = new AddImportedPollingStationRequestFaker(jobId).Generate();

        return addImportedPollingStationRequest;
    }

    public static UpdatePollingStationRequest CreateUpdatePollingStationRequest(Guid? id = null)
    {
        var updatePollingStationRequest = new UpdatePollingStationRequestFaker(id).Generate();

        return updatePollingStationRequest;
    }
    public static UpdateImportedPollingStationRequest CreateImportedUpdatePollingStationRequest(Guid? id = null)
    {
        var updateImportedPollingStationRequest = new UpdateImportedPollingStationRequestFaker(id).Generate();

        return updateImportedPollingStationRequest;
    }

    public static UpdateAssignedAddressRequest CreateUpdateAssignedAddressRequest(Guid? id = null)
    {
        var updateAssignedAddressRequest = new UpdateAssignedAddressRequestFaker(id).Generate();

        return updateAssignedAddressRequest;
    }
}
