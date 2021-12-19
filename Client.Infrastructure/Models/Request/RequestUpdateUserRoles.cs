using AuthClient.Client.Infrastructure.Models.Response;
using System.Collections.Generic;

namespace AuthClient.Client.Infrastructure.Models.Request
{
    public class RequestUpdateUserRoles
    {
        public string UserId { get; set; }

        public IList<UserRoleModel> UserRoles { get; set; }
    }
}
