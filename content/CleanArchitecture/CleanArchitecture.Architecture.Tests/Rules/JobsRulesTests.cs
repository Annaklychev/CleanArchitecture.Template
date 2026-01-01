using CleanArchitecture.Jobs;
using FluentAssertions;
using NetArchTest.Rules;

namespace CleanArchitecture.Architecture.Tests.Rules;

public class JobsRulesTests
{
    [Fact]
    public void Jobs_Should_Not_Depend_On_Infrastructure()
    {
        var result = Types
            .InAssembly(typeof(JobsAssemblyMarker).Assembly)
            .Should()
            .NotHaveDependencyOn(
                "CleanArchitecture.Infrastructure")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}