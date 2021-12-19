using System.Collections.Generic;

namespace AuthClient.Client.Infrastructure.Models.Response
{
    public class ResponsePermissionRole
    {
        public string RoleName { get; set; }

        public List<ResponsePermission> Permissions { get; set; }
    }
}