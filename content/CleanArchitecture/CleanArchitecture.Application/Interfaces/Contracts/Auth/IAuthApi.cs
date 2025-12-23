using CleanArchitecture.Application.DTOs;
using Refit;

namespace CleanArchitecture.Application.Interfaces.Contracts.Auth;

public interface IAuthApi
{
    [Post("/api/auth/login")]
    Task<AuthResultDto> Login([Body] LoginRequestDto  request);

    [Post("/api/auth/refresh")]
    Task<AuthResultDto> Refresh([Body] RefreshRequestDto request);

    [Post("/api/auth/register")]
    Task<AuthResultDto> Register([Body] RegisterRequestDto request);
}