using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.Interfaces.Auth;

namespace CleanArchitecture.Application.UseCases;

public class RefreshTokenUseCase : IRefreshTokenUseCase
{
    private readonly IRefreshTokenService _refreshTokenService;

    public RefreshTokenUseCase(IRefreshTokenService refreshTokenService)
    {
        _refreshTokenService = refreshTokenService;
    }
    public async Task<AuthResultDto> Refresh(RefreshRequestDto request)
    {
        return await _refreshTokenService.Refresh(request);
    }
}