using System.ComponentModel.DataAnnotations;
using Blazored.LocalStorage;
using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.Interfaces.Contracts.Auth;
using CleanArchitecture.WASM.Auth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Refit;

namespace CleanArchitecture.WASM.Pages;

public partial class Login : ComponentBase
{
    [Inject] public IAuthApi AuthService { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public ILocalStorageService LocalStorageService { get; set; } = null!;
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    [Inject] public Toolbelt.Blazor.I18nText.I18nText I18nText { get; set; } = null!;
    
    private LoginModel loginModel = new();
    private string? errorMessage;
    private CleanArchitecture.WASM.I18nText.AppText? _textTable;

    protected override async Task OnInitializedAsync()
    {
        _textTable = await I18nText.GetTextTableAsync<CleanArchitecture.WASM.I18nText.AppText>(this);
    }

    private async Task HandleLogin()
    {
        try
        {
            errorMessage = null;
            var result = await AuthService.Login(new(loginModel.Email, loginModel.Password));
            await ((JwtAuthenticationStateProvider)AuthenticationStateProvider).MarkUserAsAuthenticated(result);
            NavigationManager.NavigateTo("/", true);
        }
        catch (ApiException apiEx)
        {
            errorMessage = await HandleApiException(apiEx);
        }
        catch (Exception e)
        {
            errorMessage = _textTable?.ErrorsCommonUnexpectedError ?? "An error occurred";
            Console.WriteLine(e);
        }
    }

    private async Task<string> HandleApiException(ApiException apiEx)
    {
        try
        {
            var errorResponse = await apiEx.GetContentAsAsync<ErrorResponseDto>();
            if (errorResponse != null && _textTable != null)
            {
                // Map error code to property using reflection
                var message = GetLocalizedErrorMessage(errorResponse.Code);
                
                // Format with parameters if available
                if (errorResponse.Parameters != null)
                {
                    message = FormatMessageWithParameters(message, errorResponse.Parameters);
                }
                
                return message;
            }
        }
        catch
        {
            // Fallback if deserialization fails
        }

        return _textTable?.ErrorsAuthInvalidCredentials ?? "Invalid credentials";
    }

    private string GetLocalizedErrorMessage(string errorKey)
    {
        if (_textTable == null) return errorKey;

        // Convert dots to PascalCase: "errors.auth.invalid_credentials" -> "ErrorsAuthInvalidCredentials"
        var propertyName = string.Join("", errorKey.Split('.').Select(part => 
            string.Join("", part.Split('_').Select(word => 
                char.ToUpper(word[0]) + word.Substring(1)))));
        
        var property = typeof(CleanArchitecture.WASM.I18nText.AppText).GetProperty(propertyName);
        if (property != null)
        {
            return property.GetValue(_textTable)?.ToString() ?? errorKey;
        }

        return errorKey;
    }

    private static string FormatMessageWithParameters(string template, object? parameters)
    {
        if (parameters == null || string.IsNullOrEmpty(template))
            return template;

        var properties = parameters.GetType().GetProperties();
        foreach (var prop in properties)
        {
            var value = prop.GetValue(parameters)?.ToString() ?? string.Empty;
            template = template.Replace($"{{{prop.Name}}}", value, StringComparison.OrdinalIgnoreCase);
        }

        return template;
    }

    public class LoginModel
    {
        [Required] public string Email { get; set; } = null!;
        [Required] public string Password { get; set; } = null!;
    }
}