namespace CleanArchitecture.Application.DTOs;

public sealed record JwtUserDto(
    Guid Id,
    string Email);