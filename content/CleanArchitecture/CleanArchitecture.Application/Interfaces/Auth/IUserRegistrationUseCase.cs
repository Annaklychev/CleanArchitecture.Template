namespace CleanArchitecture.Application.Interfaces.Auth;

public interface IUserRegistrationUseCase
{
    Task RegisterAsync(string email, string password);
}