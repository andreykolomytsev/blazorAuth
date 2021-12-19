using System.Collections.Generic;

namespace AuthClient.Client.Infrastructure.Models.Request
{
    public class RequestPermissionRole
    {
        public List<PermissionRole> PermissionRoles { get; set; } = new();
    }

    public class PermissionRole
    {
        public string PermissionId { get; set; }
    }
}