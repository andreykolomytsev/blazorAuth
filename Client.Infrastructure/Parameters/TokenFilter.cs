using System;

namespace AuthClient.Client.Infrastructure.Parameters
{
    public class TokenFilter
    {
        public string Token { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Expires { get; set; }

        public string CreatedByIp { get; set; }

        public string CreatedByBrowser { get; set; }

        public bool? IsActive { get; set; }

        public string UserId { get; set; }
    }
}
