using System;

namespace AuthClient.Client.Infrastructure.Models.Response
{
    public class ResponseAccessToken
    {       
        public Guid Id { get; set; }

        public string Token { get; set; }

        public DateTime Expires { get; set; }

        public DateTime Created { get; set; }

        public string CreatedByIp { get; set; }

        public string CreatedByBrowser { get; set; }

        public ResponseUser User { get; set; }

        public bool IsExpired { get; set; }

        public bool IsOutDated { get; set; }

        public bool IsActive { get; set; }
    }
}
