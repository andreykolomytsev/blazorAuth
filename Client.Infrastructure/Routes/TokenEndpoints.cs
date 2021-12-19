namespace AuthClient.Client.Infrastructure.Routes
{
    public static class TokenEndpoints
    {
        public const string GetAccessTokens = "token/accesstokens";

        public const string GetRefreshTokens = "token/refreshtokens";

        public const string ChangeStatus = "token/editaccesstoken";
    }
}
