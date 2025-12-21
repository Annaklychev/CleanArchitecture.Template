using CleanArchitecture.Domain.ValueObjects;

namespace CleanArchitecture.Domain.Entities.User;

public class User : BaseEntity
{
    public Email Email { get; private set; } = null!;
    public Money Balance { get; private set; } = Money.Zero("USD");
    public string? DisplayName { get; private set; }

    private User() { }
    public User(Email email, string? displayName = null)
    {
        Email = email;
        DisplayName = displayName;
    }

    public void UpdateDisplayName(string name) => DisplayName = name;
}