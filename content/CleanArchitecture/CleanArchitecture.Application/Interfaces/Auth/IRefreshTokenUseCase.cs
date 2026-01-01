using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Interfaces.Auth;

public interface IRefreshTokenUseCase
{
    Task<AuthResultDto> Refresh(RefreshRequestDto request);
}