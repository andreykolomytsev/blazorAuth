namespace AuthClient.Client.Infrastructure.Models.Request
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}