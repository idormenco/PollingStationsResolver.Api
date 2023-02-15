using PollingStationsResolver.Geocoding.Interfaces;
using PollingStationsResolver.Geocoding.Models;
using FluentAssertions;
using NSubstitute;
using Polly.Caching;

namespace PollingStationsResolver.Geocoding.Tests;
public class GetAddressCoordinatesQueryTests
{
    [Fact]
    public async Task ExecuteAsync_ReturnsFirstFoundResponse()
    {
        // Arrange
        var firstGeocodingClient = Substitute.For<IGeocodingClient>();
        firstGeocodingClient
            .FindCoordinatesAsync(Arg.Any<string>(), Arg.Any<string>())
            .Returns(new LocationSearchResult.Found(2, 3));

        var secondGeocodingClient = Substitute.For<IGeocodingClient>();
        secondGeocodingClient
            .FindCoordinatesAsync(Arg.Any<string>(), Arg.Any<string>())
            .Returns(new LocationSearchResult.Found(4, 5));

        var firstClientFactoryMock = Substitute.For<IGeocodingClientFactory>();
        firstClientFactoryMock
            .Create()
            .Returns(firstGeocodingClient);

        var secondClientFactoryMock = Substitute.For<IGeocodingClientFactory>();
        secondClientFactoryMock
            .Create()
            .Returns(secondGeocodingClient);

        var geocodingClientFactories = new List<IGeocodingClientFactory> { firstClientFactoryMock, secondClientFactoryMock };

        var cachePolicyFactory = new CachePolicyFactory(Substitute.For<IAsyncCacheProvider>());

        var query = new GetAddressCoordinatesQuery(geocodingClientFactories, cachePolicyFactory);

        // Act
        var result = await query.ExecuteAsync("county", "address");

        // Assert
        result
            .Should()
            .BeOfType<LocationSearchResult.Found>();

        var (latitude, longitude) = result as LocationSearchResult.Found;
        latitude.Should().Be(2);
        longitude.Should().Be(3);
    }

    [Fact]
    public async Task ExecuteAsync_AllClientsFail_ReturnsNotFoundResult()
    {
        // Arrange
        var firstGeocodingClient = Substitute.For<IGeocodingClient>();
        firstGeocodingClient
            .FindCoordinatesAsync(Arg.Any<string>(), Arg.Any<string>())
            .Returns(new LocationSearchResult.Error());

        var secondGeocodingClient = Substitute.For<IGeocodingClient>();
        secondGeocodingClient
            .FindCoordinatesAsync(Arg.Any<string>(), Arg.Any<string>())
            .Returns(new LocationSearchResult.Error());

        var firstClientFactoryMock = Substitute.For<IGeocodingClientFactory>();
        firstClientFactoryMock
            .Create()
            .Returns(firstGeocodingClient);

        var secondClientFactoryMock = Substitute.For<IGeocodingClientFactory>();
        secondClientFactoryMock
            .Create()
            .Returns(secondGeocodingClient);

        var geocodingClientFactories = new List<IGeocodingClientFactory> { firstClientFactoryMock, secondClientFactoryMock };

        var cachePolicyFactory = new CachePolicyFactory(Substitute.For<IAsyncCacheProvider>());

        var query = new GetAddressCoordinatesQuery(geocodingClientFactories, cachePolicyFactory);

        // Act
        var result = await query.ExecuteAsync("county", "address");

        // Assert
        result
            .Should()
            .BeOfType<LocationSearchResult.NotFound>();
    }
}
