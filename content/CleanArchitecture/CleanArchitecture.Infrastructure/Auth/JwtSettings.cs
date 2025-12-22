namespace CleanArchitecture.Infrastructure.Auth;

public class JwtSettings
{
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public string Secret { get; init; } = null!;
    public int AccessMinutes { get; init; }
    public int RefreshDays { get; init; }
}