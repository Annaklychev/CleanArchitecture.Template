using CleanArchitecture.Application.Interfaces.Auth;

namespace CleanArchitecture.Application.UseCases;

internal sealed class UserRegistrationUseCase
    : IUserRegistrationUseCase
{
    private readonly IUserRegistrationService _registrationService;

    public UserRegistrationUseCase(IUserRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    public async Task RegisterAsync(string email, string password)
    {
        // здесь могут быть:
        // - бизнес-валидации
        // - политика паролей
        // - доменные правила
        await _registrationService.RegisterAsync(email, password);
    }
}