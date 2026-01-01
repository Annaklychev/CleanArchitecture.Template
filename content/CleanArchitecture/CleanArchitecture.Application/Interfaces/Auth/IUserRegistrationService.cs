namespace CleanArchitecture.Application.Interfaces.Auth;

public interface IUserRegistrationService
{
    Task RegisterAsync(string email, string password);
}