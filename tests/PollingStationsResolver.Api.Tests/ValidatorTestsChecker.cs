using System.Collections.Immutable;
using FluentAssertions;
using FluentValidation;
using PollingStationsResolver.Api.Features.ImportJob.Cancel;

namespace PollingStationsResolver.Api.Tests;

public class ValidatorTestsChecker
{
    [Theory]
    [MemberData(nameof(ValidatorTestCases))]
    public void Every_validator_should_have_a_test(string testName, bool testExists)
    {
        testExists.Should().BeTrue($"There should be a test called '{testName}'");
    }

    public static IEnumerable<object[]> ValidatorTestCases
    {
        get
        {
            var openGenericType = typeof(IValidator<>);
            var validators = typeof(Endpoint).Assembly.GetTypes()
                .Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition)
                .Where(type =>
                    type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == openGenericType))
                .Select(x => x.FullName!)
                .Select(x => x.Replace("PollingStationsResolver.Api.Features.Common", ""))
                .Select(x => x.Replace("PollingStationsResolver.Api.Features.", ""))
                .Select(x => x.Replace(".", ""))
                .ToImmutableArray();

            var validatorTests = typeof(EndpointTestsChecker)
                .Assembly
                .GetTypes()
                .Where(x => x.IsClass && x.IsPublic)
                .Select(x => x.FullName!)
                .Where(x => x.StartsWith("PollingStationsResolver.Api.Tests.Validators"))
                .Select(x => x.Replace("PollingStationsResolver.Api.Tests.Validators.", ""))
                .Select(x => x.Replace(".", ""))
                .ToImmutableArray();

            foreach (var validator in validators)
            {
                yield return new object[] { $"Validators\\{validator}Tests.cs", validatorTests.Contains($"{validator}Tests") };
            }
        }
    }
}
