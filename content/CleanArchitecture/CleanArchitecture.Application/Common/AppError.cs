namespace CleanArchitecture.Application.Common;

/// <summary>
/// Application layer error that carries a localization key and parameters.
/// This is returned from use cases and services to the presentation layer.
/// </summary>
public sealed record AppError(string Code, object? Parameters = null)
{
    /// <summary>
    /// Creates an error from a domain error key.
    /// </summary>
    public static AppError FromDomain(string errorKey, object? parameters = null)
        => new(errorKey, parameters);
}
