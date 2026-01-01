using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Interfaces.Auth;

public interface ILoginService
{
    Task<AuthResultDto> Login(LoginRequestDto request);
}