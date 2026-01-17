using System.Net.Mail;
using CleanArchitecture.Domain.Errors;

namespace CleanArchitecture.Domain.User.ValueObjects;

public sealed record Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException(DomainErrorKeys.Validation.Required, new { field = "email" });

        if (!value.Contains("@") || !MailAddress.TryCreate(value, out MailAddress? mailAddress))
            throw new DomainException(DomainErrorKeys.User.InvalidEmail, new { email = value });

        Value = value.ToLowerInvariant();
    }

    public override string ToString() => Value;
}