using System.Collections.Generic;

namespace AuthClient.Client.Infrastructure.Models.Request
{
    public class RequestServiceTenant
    {
        public List<TenantServiceIds> Tenants { get; set; } = new();
    }

    public class TenantServiceIds
    {
        public string TenantId { get; set; }
    }
}
