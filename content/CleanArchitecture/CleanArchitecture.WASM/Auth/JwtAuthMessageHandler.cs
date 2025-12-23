using System.Net;
using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace CleanArchitecture.WASM.Auth;


public class JwtAuthMessageHandler : DelegatingHandler
{
    private readonly ILocalStorageService _storage;

    public JwtAuthMessageHandler(ILocalStorageService storage)
    {
        _storage = storage;
    }
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _storage.GetItemAsync<string>("access_token");

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}