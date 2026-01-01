using CleanArchitecture.Domain.Shared.ValueObjects;

namespace CleanArchitecture.Domain.Tests.Shared.ValueObjects;

public class DateRangeTests
{
    [Fact]
    public void Constructor_ShouldCreateDateRange_WhenEndIsAfterStart()
    {
        // Arrange
        var start = new DateTime(2025, 1, 1);
        var end = new DateTime(2025, 1, 10);

        // Act
        var range = new DateRange(start, end);

        // Assert
        Assert.Equal(start, range.Start);
        Assert.Equal(end, range.End);
    }

    [Fact]
    public void Constructor_ShouldAllow_WhenStartEqualsEnd()
    {
        // Arrange
        var date = new DateTime(2025, 1, 1);

        // Act
        var range = new DateRange(date, date);

        // Assert
        Assert.Equal(TimeSpan.Zero, range.Duration);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenEndIsBeforeStart()
    {
        // Arrange
        var start = new DateTime(2025, 1, 10);
        var end = new DateTime(2025, 1, 1);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new DateRange(start, end));

        Assert.Equal("End date must be after start date", exception.Message);
    }

    [Fact]
    public void Duration_ShouldReturnCorrectTimeSpan()
    {
        // Arrange
        var start = new DateTime(2025, 1, 1, 10, 0, 0);
        var end = new DateTime(2025, 1, 1, 12, 30, 0);

        var range = new DateRange(start, end);

        // Act
        var duration = range.Duration;

        // Assert
        Assert.Equal(TimeSpan.FromMinutes(150), duration);
    }

    [Fact]
    public void Equality_ShouldBeValueBased()
    {
        // Arrange
        var start = new DateTime(2025, 1, 1);
        var end = new DateTime(2025, 1, 10);

        var range1 = new DateRange(start, end);
        var range2 = new DateRange(start, end);

        // Act & Assert
        Assert.Equal(range1, range2);
        Assert.True(range1 == range2);
    }

    [Fact]
    public void Equality_ShouldReturnFalse_ForDifferentRanges()
    {
        // Arrange
        var range1 = new DateRange(
            new DateTime(2025, 1, 1),
            new DateTime(2025, 1, 10));

        var range2 = new DateRange(
            new DateTime(2025, 1, 1),
            new DateTime(2025, 1, 11));

        // Act & Assert
        Assert.NotEqual(range1, range2);
    }
}