namespace PollingStationsResolver.Api.Features.ImportJob;

public class ResponseMapper : ResponseMapper<ImportJobModel, Domain.Entities.ImportJobAggregate.ImportJob>
{
    public override ImportJobModel FromEntity(Domain.Entities.ImportJobAggregate.ImportJob entity)
    {
        return new ImportJobModel
        {
            Id = entity.Id,
            FileId = entity.FileId,
            JobStatus = entity.JobStatus,
            StartedAt = entity.StartedAt,
            FinishedAt = entity.FinishedAt,
            FileName = entity.FileName
        };
    }
}
