namespace AuthClient.Client.Infrastructure.Routes
{
    public static class AuthEndpoints
    {
        public const string Get = "authenticate/authenticate";

        public const string Refresh = "authenticate/refreshtoken";

        public const string Revoke = "authenticate/revoketoken";

        public const string Check = "authenticate/checktoken";
    }
}