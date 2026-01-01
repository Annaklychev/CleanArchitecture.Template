using CleanArchitecture.Domain.Shared.ValueObjects;

namespace CleanArchitecture.Domain.Tests.Shared.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Constructor_ShouldCreateMoney_WithAmountAndCurrency()
    {
        // Arrange
        var amount = 10.5m;
        var currency = "USD";

        // Act
        var money = new Money(amount, currency);

        // Assert
        Assert.Equal(amount, money.Amount);
        Assert.Equal(currency, money.Currency);
    }

    [Fact]
    public void Zero_ShouldReturnMoney_WithZeroAmount()
    {
        // Arrange
        var currency = "EUR";

        // Act
        var money = Money.Zero(currency);

        // Assert
        Assert.Equal(0m, money.Amount);
        Assert.Equal(currency, money.Currency);
    }

    [Fact]
    public void Equality_ShouldBeValueBased()
    {
        // Arrange
        var money1 = new Money(100m, "USD");
        var money2 = new Money(100m, "USD");

        // Act & Assert
        Assert.Equal(money1, money2);
        Assert.True(money1 == money2);
    }

    [Fact]
    public void Equality_ShouldReturnFalse_WhenCurrencyDiffers()
    {
        // Arrange
        var money1 = new Money(100m, "USD");
        var money2 = new Money(100m, "EUR");

        // Act & Assert
        Assert.NotEqual(money1, money2);
    }

    [Fact]
    public void ToString_ShouldReturnAmountAndCurrency()
    {
        // Arrange
        var money = new Money(12.34m, "USD");

        // Act
        var result = money.ToString();

        // Assert
        Assert.Equal("12.34 USD", result);
    }

    [Fact]
    public void Money_ShouldAllowNegativeAmounts()
    {
        // Arrange & Act
        var money = new Money(-50m, "USD");

        // Assert
        Assert.Equal(-50m, money.Amount);
    }
}