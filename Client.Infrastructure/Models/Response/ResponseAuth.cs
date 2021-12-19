using System;

namespace AuthClient.Client.Infrastructure.Models.Response
{
    public class ResponseAuth
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiry { get; set; }

        public string RedirectURL { get; set; }
    }
}
