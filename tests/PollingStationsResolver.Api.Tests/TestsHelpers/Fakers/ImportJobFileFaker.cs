using AutoBogus;
using PollingStationsResolver.Domain.Entities.ImportJobAggregate;

namespace PollingStationsResolver.Api.Tests.TestsHelpers.Fakers;

public class ImportJobFileFaker : AutoFaker<ImportJobFile>
{
    public ImportJobFileFaker()
    {
        RuleFor(fake => fake.Id, fake => fake.Random.Guid());
        RuleFor(fake => fake.Base64File, () => "VGhpcyBpcyB0aGUgZXhjZWwgZmlsZSB5b3UgYXJlIGxvb2tpbmcgZm9y");
    }
}
