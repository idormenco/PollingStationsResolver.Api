using FastEndpoints;
using System.Collections.Immutable;
using FluentAssertions;
using PollingStationsResolver.Api.Features.ImportJob.Cancel;

namespace PollingStationsResolver.Api.Tests;

public class MapperTestsChecker
{
    [Theory]
    [MemberData(nameof(MapperTestCases))]
    public void Every_mapper_should_have_a_test(string testName, bool testExists)
    {
        testExists.Should().BeTrue($"There should be a test called '{testName}'");
    }

    public static IEnumerable<object[]> MapperTestCases
    {
        get
        {
            var mappers = typeof(Endpoint).Assembly.GetTypes()
                .Where(x => x.IsClass && x.GetInterfaces().Contains(typeof(IMapper)))
                .Select(x => x.FullName!)
                .Select(x => x.Replace("PollingStationsResolver.Api.Features.Common", ""))
                .Select(x => x.Replace("PollingStationsResolver.Api.Features.", ""))
                .Select(x => x.Replace(".", ""))
                .ToImmutableList();

            var mapperTests = typeof(EndpointTestsChecker)
                .Assembly
                .GetTypes()
                .Where(x => x.IsClass && x.IsPublic)
                .Select(x => x.FullName!)
                .Where(x => x.StartsWith("PollingStationsResolver.Api.Tests.Mappers"))
                .Select(x => x.Replace("PollingStationsResolver.Api.Tests.Mappers.", ""))
                .Select(x => x.Replace(".", ""))
                .ToImmutableArray();

            foreach (var mapper in mappers)
            {
                yield return new object[] { $"Mappers\\{mapper}Tests.cs", mapperTests.Contains($"{mapper}Tests") };
            }
        }
    }
}
