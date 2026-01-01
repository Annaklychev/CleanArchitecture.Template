using CleanArchitecture.Domain.Shared.ValueObjects;
using CleanArchitecture.Domain.User.ValueObjects;

namespace CleanArchitecture.Domain.Tests.User.Entities;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldCreateUser_WithEmail()
    {
        // Arrange
        var email = new Email("test@example.com");

        // Act
        var user = new Domain.User.Entities.User(email);

        // Assert
        Assert.Equal(email, user.Email);
        Assert.Null(user.DisplayName);
    }

    [Fact]
    public void Constructor_ShouldSetDisplayName_WhenProvided()
    {
        // Arrange
        var email = new Email("test@example.com");
        var displayName = "John Doe";

        // Act
        var user = new Domain.User.Entities.User(email, displayName);

        // Assert
        Assert.Equal(displayName, user.DisplayName);
    }

    [Fact]
    public void Constructor_ShouldInitializeBalanceWithZeroUsd()
    {
        // Arrange
        var email = new Email("test@example.com");

        // Act
        var user = new Domain.User.Entities.User(email);

        // Assert
        Assert.Equal(Money.Zero("USD"), user.Balance);
    }

    [Fact]
    public void UpdateDisplayName_ShouldChangeDisplayName()
    {
        // Arrange
        var user = new Domain.User.Entities.User(new Email("test@example.com"));

        // Act
        user.UpdateDisplayName("New Name");

        // Assert
        Assert.Equal("New Name", user.DisplayName);
    }

    [Fact]
    public void UpdateDisplayName_ShouldAllowEmptyString()
    {
        // Arrange
        var user = new Domain.User.Entities.User(new Email("test@example.com"));

        // Act
        user.UpdateDisplayName(string.Empty);

        // Assert
        Assert.Equal(string.Empty, user.DisplayName);
    }

    [Fact]
    public void User_ShouldHaveGeneratedId()
    {
        // Arrange
        var user = new Domain.User.Entities.User(new Email("test@example.com"));

        // Assert
        Assert.NotEqual(Guid.Empty, user.Id);
    }
}