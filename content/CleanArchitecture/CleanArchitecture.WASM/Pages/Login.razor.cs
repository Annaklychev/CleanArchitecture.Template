using System.ComponentModel.DataAnnotations;
using Blazored.LocalStorage;
using CleanArchitecture.Application.Interfaces.Contracts.Auth;
using CleanArchitecture.WASM.Auth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace CleanArchitecture.WASM.Pages;

public partial class Login : ComponentBase
{
    [Inject] public IAuthApi AuthService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public ILocalStorageService LocalStorageService { get; set; }
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    private LoginModel loginModel = new();
    private string? errorMessage;

    private async Task HandleLogin()
    {
        try
        {
            errorMessage = null;
            var result = await AuthService.Login(new(loginModel.Email, loginModel.Password));
            await ((JwtAuthenticationStateProvider)AuthenticationStateProvider).MarkUserAsAuthenticated(result);
            NavigationManager.NavigateTo("/", true);
        }
        catch (Exception e)
        {
            errorMessage = "Invalid credentials: " + e.Message;
            Console.WriteLine(e);
        }
    }

    public class LoginModel
    {
        [Required] public string Email { get; set; } = null!;
        [Required] public string Password { get; set; } = null!;
    }
}