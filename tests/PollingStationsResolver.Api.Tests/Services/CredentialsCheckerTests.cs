using FluentAssertions;
using PollingStationsResolver.Api.Options;
using PollingStationsResolver.Api.Services.Credentials;
using PollingStationsResolver.Api.Tests.TestsHelpers;

namespace PollingStationsResolver.Api.Tests.Services;

public class CredentialsCheckerTests
{
    [Fact]
    public void CheckCredentials_WithCorrectCredentials_ShouldReturnTrue()
    {
        // Arrange
        var options = new ApplicationAdminCredentials { UserName = "admin", Password = "password" }.AsOptions();

        var checker = new CredentialsChecker(options);

        // Act
        var result = checker.CheckCredentials("admin", "password");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CheckCredentials_WithIncorrectCredentials_ShouldReturnFalse()
    {
        // Arrange
        var options = new ApplicationAdminCredentials { UserName = "admin", Password = "password" }.AsOptions();

        var checker = new CredentialsChecker(options);

        // Act
        var result = checker.CheckCredentials("incorrect", "credentials");

        // Assert
        result.Should().BeFalse();
    }
}
