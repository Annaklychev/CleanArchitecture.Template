using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanArchitecture.Application.Abstractions.Auth;
using CleanArchitecture.Application.Interfaces.Auth;
using CleanArchitecture.Persistence.Context;
using CleanArchitecture.Persistence.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.API.Controllers;


[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;
    private readonly IJwtTokenService _jwt;
    private readonly IConfiguration _config;

    public AuthController(
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

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            throw new UnauthorizedAccessException();

        var access = _jwt.GenerateAccessToken(user);
        var refresh = _jwt.GenerateRefreshToken(user.Id);

        _db.RefreshTokens.Add(refresh);
        await _db.SaveChangesAsync();

        return Ok(new AuthResult(
            access,
            refresh.Token,
            request.Email,
            DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:AccessMinutes"]))));
    }

    [HttpPost("refresh")]
    public async Task<AuthResult> Refresh(RefreshRequest request)
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

        return new AuthResult(
            _jwt.GenerateAccessToken(user),
            newRefresh.Token,
            user.Email,
            DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:AccessMinutes"])));
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var user = new ApplicationUser { Email = request.Email, UserName = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);
    
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        // Можно сразу выдать токены
        var access = _jwt.GenerateAccessToken(user);
        var refresh = _jwt.GenerateRefreshToken(user.Id);

        _db.RefreshTokens.Add(refresh);
        await _db.SaveChangesAsync();

        return Ok(new AuthResult(access, refresh.Token,request.Email, DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:AccessMinutes"]))));
    }
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("You are authorized ✅");
    }
}