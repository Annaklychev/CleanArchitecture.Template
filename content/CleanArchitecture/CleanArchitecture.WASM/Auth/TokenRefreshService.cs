using Blazored.LocalStorage;
using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.Interfaces.Contracts.Auth;

namespace CleanArchitecture.WASM.Auth;

public interface ITokenRefreshService
{
    Task<bool> TryRefreshAsync();
}

public sealed class TokenRefreshService : ITokenRefreshService
{
    private readonly IAuthApi _authApi;
    private readonly ILocalStorageService _storage;

    public TokenRefreshService(IAuthApi authApi, ILocalStorageService storage)
    {
        _authApi = authApi;
        _storage = storage;
    }

    public async Task<bool> TryRefreshAsync()
    {
        var refresh = await _storage.GetItemAsync<string?>("refresh_token");
        if (refresh is null) return false;

        var response = await _authApi.Refresh(
            new RefreshRequestDto(refresh));

        await _storage.SetItemAsync("access_token", response.AccessToken);
        await _storage.SetItemAsync("refresh_token", response.RefreshToken);
        await _storage.SetItemAsync("userName", response.UserName);

        return true;
    }
}