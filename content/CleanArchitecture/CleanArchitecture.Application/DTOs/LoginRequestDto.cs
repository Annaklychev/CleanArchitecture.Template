namespace CleanArchitecture.Application.DTOs;

public sealed record LoginRequestDto(
    string Email,
    string Password
);