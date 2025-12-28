using System.Net;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using CleanArchitecture.Application.Interfaces.Contracts.Auth;

namespace CleanArchitecture.WASM.Auth;

public sealed class UnauthorizedHandler : DelegatingHandler
{ 
    private readonly ITokenRefreshService _refresh;
    private readonly ILocalStorageService _storage;

    public UnauthorizedHandler(
        ITokenRefreshService refresh,
        ILocalStorageService storage)
    {
        _refresh = refresh;
        _storage = storage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.Unauthorized)
            return response;

        var refreshed = await _refresh.TryRefreshAsync();
        if (!refreshed)
            return response;

        var token = await _storage.GetItemAsync<string?>("access_token");
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}