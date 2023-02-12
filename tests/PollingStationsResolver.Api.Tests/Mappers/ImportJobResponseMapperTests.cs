using FluentAssertions;
using PollingStationsResolver.Api.Features.ImportJob;
using PollingStationsResolver.Api.Tests.TestsHelpers;

namespace PollingStationsResolver.Api.Tests.Mappers;

public class ImportJobResponseMapperTests
{
    [Fact]
    public void FromEntity_ShouldMapPropertiesCorrectly()
    {
        // Arrange
        var entity = BobBuilder.CreateImportJob();
        var mapper = new ResponseMapper();

        // Act
        var result = mapper.FromEntity(entity);

        // Assert
        result.Should().BeEquivalentTo(entity, c=>c.Excluding(x=>x.File));
    }
}
