namespace PollingStationsResolver.Domain.Entities.ImportJobAggregate;

public class ImportJob : BaseEntity, IAggregateRoot
{
    public ImportJob()
    {

    }

    public ImportJob(string fileName, string base64File)
    {
        JobStatus = ImportJobStatus.NotStarted;
        FileName = fileName;
        File = new ImportJobFile(base64File);
    }

    public ImportJobFile File { get; private set; }
    public ImportJobStatus JobStatus { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? FinishedAt { get; private set; }
    public Guid FileId { get; private set; }
    public string FileName { get; private set; }

    public void Start()
    {
        JobStatus = ImportJobStatus.Started;
        StartedAt = DateTime.UtcNow;
        FinishedAt = null;
    }

    public void End()
    {
        JobStatus = ImportJobStatus.Finished;
        FinishedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        JobStatus = ImportJobStatus.Canceled;
    }
}
