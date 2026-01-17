namespace CleanArchitecture.WASM.Resources
{
    public class Errors
    {
        public AuthErrors auth { get; set; } = new();
        public UserErrors user { get; set; } = new();
        public ValidationErrors validation { get; set; } = new();
        public CommonErrors common { get; set; } = new();
    }

    public class AuthErrors
    {
        public string invalid_credentials { get; set; } = "";
        public string invalid_password { get; set; } = "";
        public string user_already_exists { get; set; } = "";
        public string token_expired { get; set; } = "";
        public string token_invalid { get; set; } = "";
        public string refresh_token_expired { get; set; } = "";
        public string refresh_token_invalid { get; set; } = "";
        public string unauthorized_access { get; set; } = "";
    }

    public class UserErrors
    {
        public string not_found { get; set; } = "";
        public string email_already_in_use { get; set; } = "";
        public string invalid_email { get; set; } = "";
        public string invalid_display_name { get; set; } = "";
    }

    public class ValidationErrors
    {
        public string required { get; set; } = "";
        public string invalid_format { get; set; } = "";
        public string min_length { get; set; } = "";
        public string max_length { get; set; } = "";
        public string out_of_range { get; set; } = "";
    }

    public class CommonErrors
    {
        public string unexpected_error { get; set; } = "";
        public string operation_failed { get; set; } = "";
        public string not_found { get; set; } = "";
        public string forbidden { get; set; } = "";
    }

    public class Auth
    {
        public string login { get; set; } = "";
        public string register { get; set; } = "";
        public string logout { get; set; } = "";
        public string email { get; set; } = "";
        public string password { get; set; } = "";
        public string confirm_password { get; set; } = "";
        public string forgot_password { get; set; } = "";
        public string remember_me { get; set; } = "";
        public string passwords_dont_match { get; set; } = "";
    }

    public class Buttons
    {
        public string save { get; set; } = "";
        public string cancel { get; set; } = "";
        public string delete { get; set; } = "";
        public string edit { get; set; } = "";
        public string submit { get; set; } = "";
        public string back { get; set; } = "";
        public string next { get; set; } = "";
        public string previous { get; set; } = "";
        public string confirm { get; set; } = "";
    }

    public class Common
    {
        public string welcome { get; set; } = "";
        public string loading { get; set; } = "";
        public string success { get; set; } = "";
        public string error { get; set; } = "";
        public string warning { get; set; } = "";
        public string info { get; set; } = "";
    }
}
