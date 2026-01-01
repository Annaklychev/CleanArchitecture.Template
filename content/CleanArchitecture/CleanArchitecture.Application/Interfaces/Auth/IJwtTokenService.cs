using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Domain.Auth.Entities;

namespace CleanArchitecture.Application.Interfaces.Auth;

public interface IJwtTokenService
{
    string GenerateAccessToken(JwtUserDto user);
    RefreshToken GenerateRefreshToken(Guid userId);
}