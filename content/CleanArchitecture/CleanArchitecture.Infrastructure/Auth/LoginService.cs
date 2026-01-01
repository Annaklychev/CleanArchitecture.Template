using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.Interfaces.Auth;
using CleanArchitecture.Persistence.Context;
using CleanArchitecture.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace CleanArchitecture.Infrastructure.Auth;

public class LoginService : ILoginService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;
    private readonly IJwtTokenService _jwt;
    private readonly IConfiguration _config;

    public LoginService(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext db,
        IJwtTokenService jwt,
        IConfiguration config)
    {
        _userManager = userManager;
        _db = db;
        _jwt = jwt;
        _config = config;
    }

    public async Task<AuthResultDto> Login(LoginRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            throw new UnauthorizedAccessException();

        var access = _jwt.GenerateAccessToken(new (user.Id, user.Email));
        var refresh = _jwt.GenerateRefreshToken(user.Id);

        _db.RefreshTokens.Add(refresh);
        await _db.SaveChangesAsync();

        return new AuthResultDto(
            access,
            refresh.Token,
            request.Email,
            DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:AccessMinutes"])));
    }
}