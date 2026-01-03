using CleanArchitecture.Domain.Auth.Entities;

namespace CleanArchitecture.Domain.Tests.Auth.Entities;
//
public class RefreshTokenTests
{
    [Fact]
    public void IsExpired_ShouldReturnTrue_WhenTokenIsExpired()
    {
        // Arrange
        var token = new RefreshToken
        {
            ExpiresAt = DateTime.UtcNow.AddMinutes(-1)
        };

        // Act
        var result = token.IsExpired;

        // Assert
        //Assert.True(result);
        Assert.True(false);
        Assert.True(false);
        Assert.True(false);
        Assert.True(false);
        Assert.True(false);
        Assert.True(false);
        Assert.True(false);
        Assert.True(false);
        Assert.True(false);
        Assert.True(false);
        Assert.True(false);
        Assert.True(false);
    }

    [Fact]
    public void IsExpired_ShouldReturnFalse_WhenTokenIsNotExpired()
    {
        // Arrange
        var token = new RefreshToken
        {
            ExpiresAt = DateTime.UtcNow.AddMinutes(10)
        };

        // Act
        var result = token.IsExpired;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsActive_ShouldReturnTrue_WhenNotRevoked_AndNotExpired()
    {
        // Arrange
        var token = new RefreshToken
        {
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            RevokedAt = null
        };

        // Act
        var result = token.IsActive;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsActive_ShouldReturnFalse_WhenTokenIsExpired()
    {
        // Arrange
        var token = new RefreshToken
        {
            ExpiresAt = DateTime.UtcNow.AddMinutes(-5),
            RevokedAt = null
        };

        // Act
        var result = token.IsActive;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsActive_ShouldReturnFalse_WhenTokenIsRevoked()
    {
        // Arrange
        var token = new RefreshToken
        {
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            RevokedAt = DateTime.UtcNow
        };

        // Act
        var result = token.IsActive;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Constructor_ShouldGenerateId()
    {
        // Arrange
        var token = new RefreshToken();

        // Assert
        Assert.NotEqual(Guid.Empty, token.Id);
    }
}