using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers;


[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    private readonly ILoginUseCase _loginUseCase;
    private readonly IUserRegistrationUseCase _userRegistrationUseCase;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IJwtTokenService _jwt;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        ILoginUseCase loginUseCase,
        IUserRegistrationUseCase userRegistrationUseCase,
        IRefreshTokenService refreshTokenService,
        IJwtTokenService jwt,
        IConfiguration config,
        ILogger<AuthController> logger)
    {
        _loginUseCase = loginUseCase;
        _userRegistrationUseCase = userRegistrationUseCase;
        _refreshTokenService = refreshTokenService;
        _jwt = jwt;
        _config = config;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _loginUseCase.ExecuteAsync(request.Email, request.Password);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<AuthResultDto> Refresh(RefreshRequestDto request)
    {
        return await _refreshTokenService.Refresh(request);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        await _userRegistrationUseCase.RegisterAsync(request.Email, request.Password);
        var result = await _loginUseCase.ExecuteAsync(request.Email, request.Password);

        return Ok(result);
    }
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("test")]
    public IActionResult Test()
    {
        _logger.LogInformation("Protected method called");
        return Ok("You are authorized ✅");
    }
}