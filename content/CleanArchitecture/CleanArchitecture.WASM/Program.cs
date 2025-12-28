using Blazored.LocalStorage;
using CleanArchitecture.Application.Interfaces.Contracts.Auth;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CleanArchitecture.WASM;
using CleanArchitecture.WASM.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredLocalStorage();
// Auth
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();

builder.Services.AddScoped<JwtAuthMessageHandler>();
builder.Services.AddScoped<UnauthorizedHandler>();

builder.Services.AddScoped<ITokenRefreshService, TokenRefreshService>();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services
    .AddRefitClient<IAuthApi>()
    .ConfigureHttpClient(c =>
        c.BaseAddress = new Uri("https://localhost:7079"));

builder.Services
    .AddRefitClient<ISecureApi>()
    .ConfigureHttpClient(c =>
        c.BaseAddress = new Uri("https://localhost:7079"))
    .AddHttpMessageHandler<JwtAuthMessageHandler>()
    .AddHttpMessageHandler<UnauthorizedHandler>();

await builder.Build().RunAsync();