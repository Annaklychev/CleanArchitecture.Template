namespace CleanArchitecture.Application.Abstractions.Auth;

public record AuthResult(
    string AccessToken,
    string RefreshToken,
    string UserName,
    DateTime AccessExpiresAt
);