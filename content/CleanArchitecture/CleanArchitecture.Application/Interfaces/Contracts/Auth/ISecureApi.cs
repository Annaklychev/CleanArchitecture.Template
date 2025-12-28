using Refit;

namespace CleanArchitecture.Application.Interfaces.Contracts.Auth;

public interface ISecureApi
{
    [Get("/api/auth/test")]
    Task<string> Test();
}