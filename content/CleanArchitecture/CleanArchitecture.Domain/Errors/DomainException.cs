namespace CleanArchitecture.Domain.Errors;

/// <summary>
/// Represents a domain exception with a localization key and optional parameters.
/// No user-facing message is stored - only the key and interpolation values.
/// </summary>
public sealed class DomainException : Exception
{
    public string ErrorKey { get; }
    public object? Parameters { get; }

    public DomainException(string errorKey, object? parameters = null)
        : base($"Domain error: {errorKey}")
    {
        ErrorKey = errorKey;
        Parameters = parameters;
    }

    public DomainException(string errorKey, object? parameters, Exception innerException)
        : base($"Domain error: {errorKey}", innerException)
    {
        ErrorKey = errorKey;
        Parameters = parameters;
    }
}
