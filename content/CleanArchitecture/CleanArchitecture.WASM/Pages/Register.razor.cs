using System.ComponentModel.DataAnnotations;
using Blazored.LocalStorage;
using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.Interfaces.Contracts.Auth;
using CleanArchitecture.WASM.Auth;
using CleanArchitecture.WASM.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Refit;

namespace CleanArchitecture.WASM.Pages;

public partial class Register : ComponentBase
{
    [Inject] public IAuthApi AuthService { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public ILocalStorageService LocalStorageService { get; set; } = null!;
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    [Inject] public LocalizationService Localization { get; set; } = null!;
    
    private RegisterModel registerModel = new();
    private string? errorMessage;

    private async Task HandleRegister()
    {
        errorMessage = null;

        if (registerModel.Password != registerModel.ConfirmPassword)
        {
            errorMessage = await Localization.GetStringAsync("auth.passwords_dont_match");
            return;
        }

        try
        {
            var result = await AuthService.Register(new(registerModel.Email, registerModel.Password));
            
            await ((JwtAuthenticationStateProvider)AuthenticationStateProvider).MarkUserAsAuthenticated(result);
            NavigationManager.NavigateTo("/", true);
        }
        catch (ApiException apiEx)
        {
            errorMessage = await HandleApiException(apiEx);
        }
        catch (Exception e)
        {
            errorMessage = await Localization.GetStringAsync("errors.common.unexpected_error");
            Console.WriteLine(e);
        }
    }

    private async Task<string> HandleApiException(ApiException apiEx)
    {
        try
        {
            var errorResponse = await apiEx.GetContentAsAsync<ErrorResponseDto>();
            if (errorResponse != null)
            {
                return await Localization.FormatAsync(errorResponse.Code, errorResponse.Parameters);
            }
        }
        catch
        {
            // Fallback if deserialization fails
        }

        return await Localization.GetStringAsync("errors.common.operation_failed");
    }

    public class RegisterModel
    {
        [Required] public string Email { get; set; } = null!;
        [Required] [MinLength(6)] public string Password { get; set; } = null!;
        [Required] public string ConfirmPassword { get; set; } = null!;
    }
}