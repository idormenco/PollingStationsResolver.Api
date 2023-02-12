namespace PollingStationsResolver.Domain.Entities.ImportJobAggregate;

public class ImportJobFile: BaseEntity
{
#pragma warning disable CS8618 // Required by Entity Framework
    private ImportJobFile()
    {

    }

    public ImportJobFile(string base64File)
    {
        Base64File = base64File;
    }

    public string Base64File { get; private set; }

}