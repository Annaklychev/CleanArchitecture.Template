using CleanArchitecture.Application.Interfaces.Auth;
using CleanArchitecture.Infrastructure.Auth;
using CleanArchitecture.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddTransient<IJwtTokenService, JwtTokenService>();
        services.AddTransient<ILoginService, LoginService>();
        services.AddTransient<IRefreshTokenService, RefreshTokenService>();
        services.AddTransient<IUserRegistrationService, IdentityUserRegistrationService>();
        //services.AddCaching(configuration);
        //services.AddMessaging(configuration);

        return services;
    }
}