using Microsoft.Extensions.Configuration;

namespace PMFWikipedia.Common
{
    public static class ConfigProvider
    {
        public static string ConnectionString { get; private set; } = string.Empty;
        public static string JwtKey { get; private set; } = string.Empty;
        public static int TokenLifetimeHours { get; private set; }
        public static string ClientUrl { get; private set; } = string.Empty;
        public static string Email { get; private set; } = string.Empty;
        public static string Password { get; private set; } = string.Empty;
        public static string EmailHost { get; private set; } = string.Empty;
        public static string ChangePasswordPage { get; private set; } = string.Empty;
        public static string Port { get; private set; } = string.Empty;
        public static int TokenExpirationTime { get; private set; } = int.MaxValue;
        public static string Issuer { get; private set; } = string.Empty;
        public static string Audience { get; private set; } = string.Empty;
        public static string ConfirmEmail { get; private set; } = string.Empty;
        public static void Setup(this IConfigurationRoot configuration)
        {
            ConnectionString = configuration["ConnectionStrings:DefaultConnection"];
            JwtKey = configuration["Jwt:Key"];
            Issuer = configuration["Jwt:Issuer"];
            Audience = configuration["Jwt:Audience"];
            TokenLifetimeHours = int.Parse(configuration["Jwt:TokenLifetimeHours"] ?? "1");

            ClientUrl = configuration["ClientUrl"];
            Email = configuration["EmailInfo:Email"];
            Password = configuration["EmailInfo:Password"];
            EmailHost = configuration["EmailInfo:EmailHost"];
            ChangePasswordPage = configuration["ChangePasswordPageUrl"];
            Port = configuration["EmailInfo:Port"];
            ConfirmEmail = configuration["ConfirmEmail"];
            TokenExpirationTime = int.Parse(configuration["TokenExpirationTime"] ?? "0");
        }
    }
}