using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.Interfaces.Auth;
using CleanArchitecture.Persistence.Context;
using CleanArchitecture.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CleanArchitecture.Infrastructure.Auth;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;
    private readonly IJwtTokenService _jwt;
    private readonly IConfiguration _config;

    public RefreshTokenService(
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
    public async Task<AuthResultDto> Refresh(RefreshRequestDto request)
    {
        var token = await _db.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == request.RefreshToken);

        if (token == null || !token.IsActive)
            throw new UnauthorizedAccessException();

        token.RevokedAt = DateTime.UtcNow;

        var newRefresh = _jwt.GenerateRefreshToken(token.UserId);
        token.ReplacedByToken = newRefresh.Token;

        _db.RefreshTokens.Add(newRefresh);
        await _db.SaveChangesAsync();

        var user = await _userManager.FindByIdAsync(token.UserId.ToString())!;

        return new AuthResultDto(
            _jwt.GenerateAccessToken(new (user.Id, user.Email)),
            newRefresh.Token,
            user.Email,
            DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:AccessMinutes"])));
    }
}