using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.Interfaces.Auth;
using Microsoft.Extensions.Configuration;

namespace CleanArchitecture.Application.UseCases;

public class LoginUseCase : ILoginUseCase
{
    private readonly ILoginService _loginService;

    public LoginUseCase(
        ILoginService loginService)
    {
        _loginService = loginService;
    }

    public async Task<AuthResultDto> ExecuteAsync(
        string email,
        string password)
    {
        return await _loginService.Login(new (email, password));
    }
}