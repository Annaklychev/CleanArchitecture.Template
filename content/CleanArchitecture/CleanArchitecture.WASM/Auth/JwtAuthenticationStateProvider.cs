using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using CleanArchitecture.Application.DTOs;
using Microsoft.AspNetCore.Components.Authorization;

namespace CleanArchitecture.WASM.Auth;


public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly ClaimsPrincipal _anonymous =
        new ClaimsPrincipal(new ClaimsIdentity());

    public JwtAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("access_token");

        if (string.IsNullOrWhiteSpace(token))
            return new AuthenticationState(_anonymous);

        var handler = new JwtSecurityTokenHandler();

        JwtSecurityToken jwt;

        try
        {
            jwt = handler.ReadJwtToken(token);
        }
        catch
        {
            await _localStorage.RemoveItemAsync("access_token");
            return new AuthenticationState(_anonymous);
        }

        if (jwt.ValidTo < DateTime.UtcNow)
        {
            await _localStorage.RemoveItemAsync("access_token");
            return new AuthenticationState(_anonymous);
        }

        var identity = new ClaimsIdentity(jwt.Claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    public async Task MarkUserAsAuthenticated(AuthResultDto response)
    {
        await _localStorage.SetItemAsync("access_token", response.AccessToken);
        await _localStorage.SetItemAsync("refresh_token", response.RefreshToken);
        await _localStorage.SetItemAsync("userName", response.UserName);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _localStorage.RemoveItemAsync("access_token");
        await _localStorage.RemoveItemAsync("refresh_token");
        await _localStorage.RemoveItemAsync("userName");
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(_anonymous)));
    }

    public async Task<string?> GetAccessTokenAsync()
        => await _localStorage.GetItemAsync<string?>("access_token");
}