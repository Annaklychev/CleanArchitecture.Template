using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Shared.ValueObjects;
using CleanArchitecture.Domain.User.ValueObjects;

namespace CleanArchitecture.Domain.User.Entities;

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