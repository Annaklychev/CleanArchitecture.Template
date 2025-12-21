using System.Net.Mail;

namespace CleanArchitecture.Domain.ValueObjects;

public sealed record Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty");

        if (!value.Contains("@") || !MailAddress.TryCreate(value, out MailAddress? mailAddress))
            throw new ArgumentException("Invalid email format");

        Value = value.ToLowerInvariant();
    }

    public override string ToString() => Value;
}