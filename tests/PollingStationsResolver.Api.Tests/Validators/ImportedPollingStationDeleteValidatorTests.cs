using FluentValidation.TestHelper;
using PollingStationsResolver.Api.Features.ImportedPollingStation.Delete;

namespace PollingStationsResolver.Api.Tests.Validators;

public class ImportedPollingStationDeleteValidatorTests
{
    private readonly Validator _validator;

    public ImportedPollingStationDeleteValidatorTests()
    {
        _validator = new Validator();
    }


    [Fact]
    public void Id_ShouldNotBeEmpty()
    {
        // Arrange
        var request = new Request() { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void JobId_ShouldNotBeEmpty()
    {
        // Arrange
        var request = new Request() { JobId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.JobId);
    }
}
