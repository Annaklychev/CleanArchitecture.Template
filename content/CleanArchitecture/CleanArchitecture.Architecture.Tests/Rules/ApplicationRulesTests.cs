using CleanArchitecture.Application;
using FluentAssertions;
using NetArchTest.Rules;

namespace CleanArchitecture.Architecture.Tests.Rules;

public class ApplicationRulesTests
{
    [Fact]
    public void Application_Should_Not_Depend_On_Infrastructure()
    {
        var result = Types
            .InAssembly(typeof(ApplicationAssemblyMarker).Assembly)
            .Should()
            .NotHaveDependencyOn(
                "CleanArchitecture.Infrastructure")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}