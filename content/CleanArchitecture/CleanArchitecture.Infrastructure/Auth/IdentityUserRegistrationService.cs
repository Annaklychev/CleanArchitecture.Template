using CleanArchitecture.Application.Interfaces.Auth;
using CleanArchitecture.Persistence.Identity;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Auth;

internal sealed class IdentityUserRegistrationService
    : IUserRegistrationService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityUserRegistrationService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task RegisterAsync(string email, string password)
    {
        var user = new ApplicationUser
        {
            Email = email,
            UserName = email
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            throw new Exception(string.Join("; ", result.Errors));
    }
}