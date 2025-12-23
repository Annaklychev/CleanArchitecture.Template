using System.ComponentModel.DataAnnotations;
using Blazored.LocalStorage;
using CleanArchitecture.Application.Interfaces.Contracts.Auth;
using CleanArchitecture.WASM.Auth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace CleanArchitecture.WASM.Pages;

public partial class Register : ComponentBase
{
    [Inject] public IAuthApi AuthService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public ILocalStorageService LocalStorageService { get; set; }
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    private RegisterModel registerModel = new();
    private string? errorMessage;

    private async Task HandleRegister()
    {
        errorMessage = null;

        if (registerModel.Password != registerModel.ConfirmPassword)
        {
            errorMessage = "Passwords do not match";
            return;
        }

        try
        {
            var result = await AuthService.Register(new(registerModel.Email, registerModel.Password));
            
            await ((JwtAuthenticationStateProvider)AuthenticationStateProvider).MarkUserAsAuthenticated(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            errorMessage = "Registration failed: " + e.Message;
            throw;
        }
    }

    public class RegisterModel
    {
        [Required] public string Email { get; set; } = null!;
        [Required] [MinLength(6)] public string Password { get; set; } = null!;
        [Required] public string ConfirmPassword { get; set; } = null!;
    }
}