using AutoBogus;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Api.Tests.TestsHelpers.Fakers;

public class ImportJobFaker : AutoFaker<ImportJob>
{
    public ImportJobFaker(Guid id, ImportJobStatus? status = null)
    {
        RuleFor(fake => fake.Id, () => id);
        RuleFor(fake => fake.FileName, fake => fake.System.FileName());
        RuleFor(fake => fake.FileId, fake => fake.Random.Guid());
        RuleFor(fake => fake.File, () => new ImportJobFileFaker().Generate());
        RuleFor(fake => fake.JobStatus, fake => status ?? fake.PickRandom(ImportJobStatus.NotStarted,
            ImportJobStatus.Started,
            ImportJobStatus.Finished,
            ImportJobStatus.Canceled,
            ImportJobStatus.Imported));
    }
}
