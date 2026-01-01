using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Interfaces.Auth;

public interface ILoginUseCase
{
    Task<AuthResultDto> ExecuteAsync(string email, string password);
}