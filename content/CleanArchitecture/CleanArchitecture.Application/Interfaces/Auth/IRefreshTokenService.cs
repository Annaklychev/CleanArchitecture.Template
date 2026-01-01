using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Interfaces.Auth;

public interface IRefreshTokenService
{
    Task<AuthResultDto> Refresh(RefreshRequestDto request);
}