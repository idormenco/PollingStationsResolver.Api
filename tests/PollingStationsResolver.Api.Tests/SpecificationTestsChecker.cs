using System.Collections.Immutable;
using Ardalis.Specification;
using FluentAssertions;
using PollingStationsResolver.Domain.Specifications;

namespace PollingStationsResolver.Api.Tests;

public class SpecificationTestsChecker
{
    [Theory]
    [MemberData(nameof(EndpointTestCases))]
    public void Every_specification_should_have_a_test(string testName, bool testExists)
    {
        testExists.Should().BeTrue($"There should be a test called '{testName}'");
    }

    public static IEnumerable<object[]> EndpointTestCases
    {
        get
        {
            var openGenericType = typeof(ISpecification<>);
            var specifications = typeof(CurrentImportJobInProgressSpecification).Assembly.GetTypes()
                .Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition)
                .Where(type =>
                    type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == openGenericType))
                .Select(x => x.FullName!)
                .Select(x => x.Replace("PollingStationsResolver.Domain.Specifications", ""))
                .Select(x => x.Replace(".", ""))
                .ToImmutableArray();

            var specificationTests = typeof(EndpointTestsChecker)
                .Assembly
                .GetTypes()
                .Where(x => x.IsClass && x.IsPublic)
                .Select(x => x.FullName!)
                .Where(x => x.StartsWith("PollingStationsResolver.Api.Tests.Specifications"))
                .Select(x => x.Replace("PollingStationsResolver.Api.Tests.Specifications.", ""))
                .Select(x => x.Replace(".", ""))
                .ToImmutableArray();

            foreach (var specification in specifications)
            {
                yield return new object[] { $"Specifications\\{specification}Tests.cs", specificationTests.Contains($"{specification}Tests") };
            }
        }
    }
}
