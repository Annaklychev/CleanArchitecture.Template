using Toolbelt.Blazor.I18nText;

namespace CleanArchitecture.WASM.Localization;

/// <summary>
/// Helper service to format localized messages with parameters.
/// Handles placeholder interpolation like {field}, {attempts}, etc.
/// </summary>
public sealed class LocalizationService
{
    private readonly Toolbelt.Blazor.I18nText.I18nText _i18n;

    public LocalizationService(Toolbelt.Blazor.I18nText.I18nText i18n)
    {
        _i18n = i18n;
    }

    /// <summary>
    /// Gets a localized string by error key path using reflection.
    /// Example: "errors.auth.invalid_password"
    /// </summary>
    public async Task<string> GetStringAsync(string key)
    {
        try
        {
            var textTable = await _i18n.GetTextTableAsync<Resources.en>(null!);
            var parts = key.Split('.');
            object? current = textTable;

            foreach (var part in parts)
            {
                if (current == null) return key;

                var property = current.GetType().GetProperty(part);
                if (property == null) return key;
                
                current = property.GetValue(current);
            }

            return current?.ToString() ?? key;
        }
        catch
        {
            return key;
        }
    }

    /// <summary>
    /// Gets and formats a localized string with parameter interpolation.
    /// </summary>
    public async Task<string> FormatAsync(string key, object? parameters)
    {
        var template = await GetStringAsync(key);
        
        if (parameters == null) return template;

        return FormatTemplate(template, parameters);
    }

    /// <summary>
    /// Synchronous version that formats a template directly (when template is already known)
    /// </summary>
    public string Format(string template, object? parameters)
    {
        if (parameters == null) return template;
        return FormatTemplate(template, parameters);
    }

    private static string FormatTemplate(string template, object parameters)
    {
        var properties = parameters.GetType().GetProperties();
        
        foreach (var prop in properties)
        {
            var value = prop.GetValue(parameters)?.ToString() ?? string.Empty;
            template = System.Text.RegularExpressions.Regex.Replace(
                template,
                @"\{(\w+)\}",
                match =>
                {
                    var placeholder = match.Groups[1].Value;
                    return placeholder.Equals(prop.Name, StringComparison.OrdinalIgnoreCase)
                        ? value
                        : match.Value;
                });
        }

        return template;
    }
}
