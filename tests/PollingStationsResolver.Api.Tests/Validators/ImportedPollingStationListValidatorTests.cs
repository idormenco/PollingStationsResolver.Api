using FluentValidation.TestHelper;
using PollingStationsResolver.Api.Features.ImportedPollingStation.List;

namespace PollingStationsResolver.Api.Tests.Validators;

public class ImportedPollingStationListValidatorTests
{

    [Fact]
    public void JobId_ShouldNotBeEmpty()
    {
        // Arrange
        var validator = new Validator();
        var request = new Request() { JobId = Guid.Empty };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.JobId);
    }

    [Theory]
    [InlineData(9)]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    public void PageSize_ShouldBeInclusiveBetween10And100(int pageSize)
    {
        // Arrange
        var validator = new Validator();
        var request = new Request { PageSize = pageSize };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Fact]
    public void Page_ShouldBeGreaterThanOrEqualTo0()
    {
        // Arrange
        var validator = new Validator();
        var request = new Request { Page = -1 };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Page);
    }

}
