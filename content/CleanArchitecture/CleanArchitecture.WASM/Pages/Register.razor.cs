using System.ComponentModel.DataAnnotations;
using Blazored.LocalStorage;
using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.Interfaces.Contracts.Auth;
using CleanArchitecture.WASM.Auth;
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
    [Inject] public Toolbelt.Blazor.I18nText.I18nText I18nText { get; set; } = null!;
    
    private RegisterModel registerModel = new();
    private string? errorMessage;
    private CleanArchitecture.WASM.I18nText.AppText? _textTable;

    protected override async Task OnInitializedAsync()
    {
        _textTable = await I18nText.GetTextTableAsync<CleanArchitecture.WASM.I18nText.AppText>(this);
    }

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
                var message = GetLocalizedErrorMessage(errorResponse.Code);
                
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

        return _textTable?.ErrorsCommonOperationFailed ?? "Operation failed";
    }

    private string GetLocalizedErrorMessage(string errorKey)
    {
        if (_textTable == null) return errorKey;

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

    public class RegisterModel
    {
        [Required] public string Email { get; set; } = null!;
        [Required] [MinLength(6)] public string Password { get; set; } = null!;
        [Required] public string ConfirmPassword { get; set; } = null!;
    }
}