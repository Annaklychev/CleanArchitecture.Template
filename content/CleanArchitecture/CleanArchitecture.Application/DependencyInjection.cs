using CleanArchitecture.Application.Interfaces.Auth;
using CleanArchitecture.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<ILoginUseCase, LoginUseCase>();
        services.AddTransient<IRefreshTokenUseCase, RefreshTokenUseCase>();
        services.AddTransient<IUserRegistrationUseCase, UserRegistrationUseCase>();
        
#warning "AutoMapper, validators — Later"
        return services;
    }
}