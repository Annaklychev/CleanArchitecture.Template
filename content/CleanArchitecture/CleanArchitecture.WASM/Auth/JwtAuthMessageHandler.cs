using System.Net;
using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace CleanArchitecture.WASM.Auth;


public class JwtAuthMessageHandler : DelegatingHandler
{
    private readonly IAuthService _authService;

    public JwtAuthMessageHandler(IAuthService authService)
    {
        _authService = authService;
    }
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _authService.GetAccessTokenAsync();
        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            // try refresh
            var success = await _authService.TryRefreshTokenAsync();
            if (success)
            {
                token = await _authService.GetAccessTokenAsync();
                // repeat the request with a new token
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await base.SendAsync(request, cancellationToken);
            }
            else
            {
                // the token has not been updated → logout
                await _authService.LogoutAsync();
            }
        }

        return response;
    }
}