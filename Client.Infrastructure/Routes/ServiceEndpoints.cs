namespace AuthClient.Client.Infrastructure.Routes
{
    public static class ServiceEndpoints
    {
        public const string GetAll = "service";

        public const string Create = "service";

        public static string GetById(string msId)
        {
            return $"service/{msId}";
        }

        public static string GetUserById(string msId)
        {
            return $"service/users/{msId}";
        }

        public static string GetTenantById(string msId)
        {
            return $"service/tenants/{msId}";
        }
    }
}
