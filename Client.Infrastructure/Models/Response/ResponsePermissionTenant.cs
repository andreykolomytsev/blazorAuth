using System.Collections.Generic;

namespace AuthClient.Client.Infrastructure.Models.Response
{
    public class ResponsePermissionTenant
    {
        public string TenantName { get; set; }

        public List<ResponsePermission> Permissions { get; set; }
    }
}
