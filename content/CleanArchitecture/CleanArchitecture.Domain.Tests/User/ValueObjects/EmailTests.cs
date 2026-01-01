using CleanArchitecture.Domain.User.ValueObjects;

namespace CleanArchitecture.Domain.Tests.User.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData("test@example.com")]
    [InlineData("USER@EXAMPLE.COM")]
    [InlineData("user.name+tag@example.co.uk")]
    public void Constructor_ShouldCreateEmail_ForValidAddresses(string input)
    {
        // Act
        var email = new Email(input);

        // Assert
        Assert.Equal(input.ToLowerInvariant(), email.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrow_WhenValueIsNullOrEmpty(string? input)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Email(input!));
        Assert.Equal("Email cannot be empty", ex.Message);
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("invalid@")]
    [InlineData("@example.com")]
    [InlineData("test@@example.com")]
    public void Constructor_ShouldThrow_WhenEmailFormatIsInvalid(string input)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Email(input));
        Assert.Equal("Invalid email format", ex.Message);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var email = new Email("Test@Example.com");

        // Act
        var result = email.ToString();

        // Assert
        Assert.Equal("test@example.com", result);
    }

    [Fact]
    public void Equality_ShouldBeValueBased()
    {
        // Arrange
        var email1 = new Email("TEST@EXAMPLE.COM");
        var email2 = new Email("test@example.com");

        // Act & Assert
        Assert.Equal(email1, email2);
        Assert.True(email1 == email2);
    }

    [Fact]
    public void Equality_ShouldReturnFalse_ForDifferentEmails()
    {
        // Arrange
        var email1 = new Email("test1@example.com");
        var email2 = new Email("test2@example.com");

        // Act & Assert
        Assert.NotEqual(email1, email2);
    }
}