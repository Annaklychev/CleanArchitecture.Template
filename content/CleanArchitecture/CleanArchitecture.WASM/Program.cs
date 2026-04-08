using Blazored.LocalStorage;
using CleanArchitecture.Application.Interfaces.Contracts.Auth;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CleanArchitecture.WASM;
using CleanArchitecture.WASM.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Refit;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using System.Globalization;
using Toolbelt.Blazor.I18nText;

// Set default culture to English
var culture = new CultureInfo("en");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

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

#pragma warning disable CS0618 // Type or member is obsolete
builder.Services.AddI18nText(options =>
{
    options.PersistanceLevel = PersistanceLevel.Cookie;
});
#pragma warning restore CS0618 // Type or member is obsolete

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

var host = builder.Build();

await host.RunAsync();