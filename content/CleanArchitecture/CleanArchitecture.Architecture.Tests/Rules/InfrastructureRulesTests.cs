using CleanArchitecture.Infrastructure;
using FluentAssertions;
using NetArchTest.Rules;

namespace CleanArchitecture.Architecture.Tests.Rules;

public class InfrastructureRulesTests
{
    [Fact]
    public void Infrastructure_Should_Not_Depend_On_UI()
    {
        var result = Types
            .InAssembly(typeof(InfrastructureAssemblyMarker).Assembly)
            .Should()
            .NotHaveDependencyOnAny(
                "CleanArchitecture.API",
                "CleanArchitecture.WASM")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}