using System.Globalization;

namespace CleanArchitecture.Domain.Shared.ValueObjects;

public sealed record Money(decimal Amount, string Currency)
{
    public static Money Zero(string currency) => new(0m, currency);

    public override string ToString() => $"{Amount.ToString(CultureInfo.InvariantCulture)} {Currency}";
}