namespace AuthClient.Client.Infrastructure.Routes
{
    public static class UserEndpoints
    {
        public const string GetAll = "user";

        public const string Create = "user";

        public static string GetById(string userId)
        {
            return $"user/{userId}";
        }

        public static string GetUserRoles(string userId)
        {
            return $"user/roles/{userId}";
        }

        public static string GetUsersByParentUser(string userId)
        {
            return $"user/childusers/{userId}";
        }
    }
}