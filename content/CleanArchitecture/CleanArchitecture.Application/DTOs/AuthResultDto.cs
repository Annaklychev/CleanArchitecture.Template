namespace CleanArchitecture.Application.DTOs;

public record AuthResultDto(
    string AccessToken,
    string RefreshToken,
    string UserName,
    DateTime AccessExpiresAt);