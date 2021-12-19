using System.Collections.Generic;

namespace AuthClient.Client.Infrastructure.Models.Request
{
    public class RequestServiceUser
    {
        public List<UserServiceIds> Users { get; set; } = new();
    }

    public class UserServiceIds
    {
        public string UserId { get; set; }
    }
}
