using System.Net.Http.Json;
using Blazored.LocalStorage;
using CleanArchitecture.Application.Abstractions.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace CleanArchitecture.WASM.Auth;

public interface IAuthService
{
    Task LoginAsync(string email, string password);
    Task<bool> RegisterAsync(string email, string password);
    Task LogoutAsync();
    Task<string?> GetAccessTokenAsync();
    Task<bool> TryRefreshTokenAsync();
}

public class AuthService : IAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalStorageService _localStorage;
    private readonly JwtAuthenticationStateProvider _authState;

    public AuthService(
        IHttpClientFactory httpClientFactory,
        AuthenticationStateProvider authState, ILocalStorageService localStorage)
    {
        _httpClientFactory = httpClientFactory;
        _localStorage = localStorage;
        _authState = (JwtAuthenticationStateProvider)authState;
    }

    public async Task LoginAsync(string email, string password)
    {
        var httpClient = _httpClientFactory.CreateClient("Api");
        var response = await httpClient.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password});

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AuthResult>();

        await _authState.MarkUserAsAuthenticated(result);
    }
    
    public async Task<bool> RegisterAsync(string email, string password)
    {
        var httpClient = _httpClientFactory.CreateClient("Api");
        var response = await httpClient.PostAsJsonAsync("api/auth/register", new { Email = email, Password = password });
        if (!response.IsSuccessStatusCode) return false;

        var result = await response.Content.ReadFromJsonAsync<AuthResult>();
        if (result is null) return false;

        await _localStorage.SetItemAsync("access_token", result.AccessToken);
        await _localStorage.SetItemAsync("refresh_token", result.RefreshToken);
        await _localStorage.SetItemAsync("userName", result.UserName);

        return true;
    }

    public async Task<bool> TryRefreshTokenAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("Api");
        var refreshToken = await _localStorage.GetItemAsync<string>("refresh_token");
        if (string.IsNullOrEmpty(refreshToken)) return false;

        var response = await httpClient.PostAsJsonAsync("api/auth/refresh", new { RefreshToken = refreshToken });
        if (!response.IsSuccessStatusCode) return false;

        var result = await response.Content.ReadFromJsonAsync<AuthResult>();
        if (result is null) return false;

        await _localStorage.SetItemAsync("access_token", result.AccessToken);
        await _localStorage.SetItemAsync("refresh_token", result.RefreshToken);
        await _localStorage.SetItemAsync("userName", result.UserName);

        return true;
    }
    public async Task LogoutAsync()
    {
        await _authState.MarkUserAsLoggedOut();
    }
    public async Task<string?> GetAccessTokenAsync()
        => await _localStorage.GetItemAsync<string>("access_token");
}

public record LoginResponse(string Token, string UserName);