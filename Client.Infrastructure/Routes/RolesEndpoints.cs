namespace AuthClient.Client.Infrastructure.Routes
{
    public static class RolesEndpoints
    {
        public const string GetAll = "role";

        public const string Create = "role";

        public static string GetById(string roleId)
        {
            return $"role/{roleId}";
        }

        public static string GetPermissionsById(string roleId)
        {
            return $"role/permissions/{roleId}";
        }
    }
}