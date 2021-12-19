namespace AuthClient.Client.Infrastructure.Routes
{
    public static class PermissionEndpoints
    {
        public const string GetAll = "permission";

        public const string Create = "permission";

        public static string GetById(string permissionId)
        {
            return $"permission/{permissionId}";
        }
    }
}
