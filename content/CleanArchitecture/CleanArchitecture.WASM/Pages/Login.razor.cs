using System.ComponentModel.DataAnnotations;
using CleanArchitecture.WASM.Auth;
using Microsoft.AspNetCore.Components;

namespace CleanArchitecture.WASM.Pages;

public partial class Login : ComponentBase
{
    [Inject] public IAuthService AuthService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    private LoginModel loginModel = new();
    private string? errorMessage;

    private async Task HandleLogin()
    {
        try
        {
            errorMessage = null;
            await AuthService.LoginAsync(loginModel.Email, loginModel.Password);
        
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