using CleanArchitecture.Domain;
using FluentAssertions;
using NetArchTest.Rules;

namespace CleanArchitecture.Architecture.Tests.Rules;

public class DomainRulesTests
{
    [Fact]
    public void Domain_Should_Not_Depend_On_Other_Layers()
    {
        var result = Types
            .InAssembly(typeof(DomainAssemblyMarker).Assembly)
            .Should()
            .NotHaveDependencyOnAny(
                "CleanArchitecture.Application",
                "CleanArchitecture.Infrastructure",
                "CleanArchitecture.Jobs",
                "Microsoft.EntityFrameworkCore",
                "Microsoft.AspNetCore")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}