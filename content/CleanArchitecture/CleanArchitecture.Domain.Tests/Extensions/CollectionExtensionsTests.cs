using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Extensions;

namespace CleanArchitecture.Domain.Tests.Extensions;

public class CollectionExtensionsTests
{
    // -----------------------------
    // IsEmpty
    // -----------------------------

    [Fact]
    public void IsEmpty_ShouldReturnTrue_WhenCollectionIsEmpty()
    {
        // Arrange
        var collection = new List<int>();

        // Act
        var result = collection.IsEmpty();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_ShouldReturnFalse_WhenCollectionHasItems()
    {
        // Arrange
        var collection = new List<int> { 1 };

        // Act
        var result = collection.IsEmpty();

        // Assert
        Assert.False(result);
    }

    // -----------------------------
    // AllMatch
    // -----------------------------

    [Fact]
    public void AllMatch_ShouldReturnTrue_WhenAllItemsMatchPredicate()
    {
        // Arrange
        var collection = new[] { 2, 4, 6 };

        // Act
        var result = collection.AllMatch(x => x % 2 == 0);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AllMatch_ShouldReturnFalse_WhenAnyItemDoesNotMatchPredicate()
    {
        // Arrange
        var collection = new[] { 2, 3, 4 };

        // Act
        var result = collection.AllMatch(x => x % 2 == 0);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AllMatch_ShouldReturnTrue_ForEmptyCollection()
    {
        // Arrange
        var collection = Array.Empty<int>();

        // Act
        var result = collection.AllMatch(x => x > 0);

        // Assert
        Assert.True(result); // стандартное поведение LINQ All
    }

    // -----------------------------
    // FindById
    // -----------------------------

    [Fact]
    public void FindById_ShouldReturnEntity_WhenEntityExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var collection = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "A" },
            new() { Id = id, Name = "B" }
        };

        // Act
        var result = collection.FindById(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result!.Id);
        Assert.Equal("B", result.Name);
    }

    [Fact]
    public void FindById_ShouldReturnNull_WhenEntityDoesNotExist()
    {
        // Arrange
        var collection = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid() }
        };

        // Act
        var result = collection.FindById(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FindById_ShouldReturnNull_ForEmptyCollection()
    {
        // Arrange
        var collection = new List<TestEntity>();

        // Act
        var result = collection.FindById(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }
}

public class TestEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}