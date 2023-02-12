using FluentAssertions;
using PollingStationsResolver.Api.Features.ImportedPollingStation;
using PollingStationsResolver.Api.Tests.TestsHelpers;

namespace PollingStationsResolver.Api.Tests.Mappers;

public class ImportedPollingStationResponseMapperTests
{
    [Fact]
    public void FromEntity_ShouldMapPropertiesCorrectly()
    {
        // Arrange
        var entity = BobBuilder.CreateImportedPollingStation();
        var mapper = new ResponseMapper();

        // Act
        var result = mapper.FromEntity(entity);

        // Assert
        result.Should().BeEquivalentTo(entity);
    }
}
