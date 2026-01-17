namespace CleanArchitecture.Domain.Errors;

/// <summary>
/// Central registry of all domain error keys for localization.
/// Keys follow hierarchical dot notation for organized translation files.
/// </summary>
public static class DomainErrorKeys
{
    public static class Auth
    {
        public const string InvalidCredentials = "errors.auth.invalid_credentials";
        public const string InvalidPassword = "errors.auth.invalid_password";
        public const string UserAlreadyExists = "errors.auth.user_already_exists";
        public const string TokenExpired = "errors.auth.token_expired";
        public const string TokenInvalid = "errors.auth.token_invalid";
        public const string RefreshTokenExpired = "errors.auth.refresh_token_expired";
        public const string RefreshTokenInvalid = "errors.auth.refresh_token_invalid";
        public const string UnauthorizedAccess = "errors.auth.unauthorized_access";
    }

    public static class User
    {
        public const string NotFound = "errors.user.not_found";
        public const string EmailAlreadyInUse = "errors.user.email_already_in_use";
        public const string InvalidEmail = "errors.user.invalid_email";
        public const string InvalidDisplayName = "errors.user.invalid_display_name";
    }

    public static class Validation
    {
        public const string Required = "errors.validation.required";
        public const string InvalidFormat = "errors.validation.invalid_format";
        public const string MinLength = "errors.validation.min_length";
        public const string MaxLength = "errors.validation.max_length";
        public const string OutOfRange = "errors.validation.out_of_range";
    }

    public static class Common
    {
        public const string UnexpectedError = "errors.common.unexpected_error";
        public const string OperationFailed = "errors.common.operation_failed";
        public const string NotFound = "errors.common.not_found";
        public const string Forbidden = "errors.common.forbidden";
    }
}
