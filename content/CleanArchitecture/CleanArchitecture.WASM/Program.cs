using Blazored.LocalStorage;
using CleanArchitecture.Application.Interfaces.Contracts.Auth;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CleanArchitecture.WASM;
using CleanArchitecture.WASM.Auth;
using CleanArchitecture.WASM.Localization;
using Microsoft.AspNetCore.Components.Authorization;
using Refit;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var culture = new System.Globalization.CultureInfo("en");
System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture;


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

// Localization - Toolbelt.Blazor.I18nText v14
builder.Services.AddI18nText();
builder.Services.AddScoped<LocalizationService>();

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