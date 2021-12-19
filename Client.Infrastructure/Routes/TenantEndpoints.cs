namespace AuthClient.Client.Infrastructure.Routes
{
    public static class TenantEndpoints
    {
        public const string GetAll = "tenant";

        public const string Create = "tenant";

        public static string GetById(string tenantId)
        {
            return $"tenant/{tenantId}";
        }

        public static string GetPermissionsById(string tenantId)
        {
            return $"tenant/permissions/{tenantId}";
        }
    }
}
