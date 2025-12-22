using System.ComponentModel.DataAnnotations;
using CleanArchitecture.WASM.Auth;
using Microsoft.AspNetCore.Components;

namespace CleanArchitecture.WASM.Pages;

public partial class Register : ComponentBase
{
    [Inject] public IAuthService AuthService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
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

        var success = await AuthService.RegisterAsync(registerModel.Email, registerModel.Password);
        if (success)
        {
            NavigationManager.NavigateTo("/", true);
        }
        else
        {
            errorMessage = "Registration failed";
        }
    }

    public class RegisterModel
    {
        [Required] public string Email { get; set; } = null!;
        [Required] [MinLength(6)] public string Password { get; set; } = null!;
        [Required] public string ConfirmPassword { get; set; } = null!;
    }
}