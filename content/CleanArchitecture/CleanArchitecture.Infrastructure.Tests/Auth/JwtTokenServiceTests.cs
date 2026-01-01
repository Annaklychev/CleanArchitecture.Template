using System.IdentityModel.Tokens.Jwt;
using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Infrastructure.Auth;
using FluentAssertions;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Infrastructure.Tests.Auth;

public class JwtTokenServiceTests
{
    private readonly JwtTokenService _service;

    public JwtTokenServiceTests()
    {
        var settings = Options.Create(new JwtSettings
        {
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            Secret = "SuperSecretKey123456789012345678901234567890",
            AccessMinutes = 60,
            RefreshDays = 7
        });

        _service = new JwtTokenService(settings);
    }

    [Fact]
    public void GenerateAccessToken_ShouldReturnValidJwt()
    {
        var user = new JwtUserDto(Guid.NewGuid(),  "test@example.com");

        var token = _service.GenerateAccessToken(user);

        token.Should().NotBeNullOrEmpty();

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        jwt.Issuer.Should().Be("TestIssuer");
        jwt.Audiences.Should().Contain("TestAudience");
        jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == user.Id.ToString());
        jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == user.Email);
    }

    [Fact]
    public void GenerateRefreshToken_ShouldReturnValidRefreshToken()
    {
        var userId = Guid.NewGuid();

        var refreshToken = _service.GenerateRefreshToken(userId);

        refreshToken.Should().NotBeNull();
        refreshToken.UserId.Should().Be(userId);
        refreshToken.Token.Should().NotBeNullOrEmpty();
        refreshToken.Token.Length.Should().BeGreaterThan(64);
        refreshToken.ExpiresAt.Should().BeAfter(refreshToken.CreatedAt);
        refreshToken.ExpiresAt.Should().BeCloseTo(DateTime.UtcNow.AddDays(7), TimeSpan.FromSeconds(5));
    }
}