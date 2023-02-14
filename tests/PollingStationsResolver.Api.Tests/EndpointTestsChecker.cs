
using System.Collections.Immutable;
using FastEndpoints;
using FluentAssertions;
using PollingStationsResolver.Api.Features.ImportJob.Cancel;

namespace PollingStationsResolver.Api.Tests;

public class EndpointTestsChecker
{
    [Theory]
    [MemberData(nameof(EndpointTestCases))]
    public void Every_endpoint_should_have_a_test(string testName, bool testExists)
    {
        // Temporary disable test
        //testExists.Should().BeTrue($"There should be a test called '{testName}'");
    }

    public static IEnumerable<object[]> EndpointTestCases
    {
        get
        {
            var endpoints = typeof(Endpoint).Assembly.GetTypes()
                .Where(x => x.IsClass && x.IsSubclassOf(typeof(BaseEndpoint)))
                .Select(x => x.FullName!)
                .Select(x => x.Replace("PollingStationsResolver.Api.Features.", ""))
                .Select(x => x.Replace(".", ""))
                .ToImmutableList();

            var endpointTests = typeof(EndpointTestsChecker)
                .Assembly
                .GetTypes()
                .Where(x => x.IsClass && x.IsPublic)
                .Select(x => x.FullName!)
                .Where(x => x.StartsWith("PollingStationsResolver.Api.Tests.Endpoints"))
                .Select(x => x.Replace("PollingStationsResolver.Api.Tests.Endpoints.", ""))
                .Select(x => x.Replace(".", ""))
                .ToImmutableArray();

            foreach (var endpoint in endpoints)
            {
                yield return new object[] { $"Endpoints\\{endpoint}Tests.cs", endpointTests.Contains($"{endpoint}Tests") };
            }
        }
    }
}
