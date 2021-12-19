using System.Collections.Generic;

namespace AuthClient.Client.Infrastructure.Models.Request
{
    public class RequestPermissionTenant
    {
        public List<PermissionTenant> PermissionTenants { get; set; } = new();
    }

    public class PermissionTenant
    {
        public string PermissionId { get; set; }
    }
}
