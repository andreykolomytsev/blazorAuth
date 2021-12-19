using System.Threading.Tasks;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Infrastructure.Models.Request;
using AuthClient.Client.Infrastructure.Models.Response;

namespace AuthClient.Client.Infrastructure.Managers.Identity.Tenants
{
    public interface ITenantManager : IManager
    {
        Task<PaginatedResult<ResponseTenant>> GetAllTenantsAsync(int pageNumber, int pageSize);
        Task<IResult<ResponseTenant>> GetTenantByIdAsync(string tenantId);
        Task<IResult<ResponseTenant>> CreateTenantAsync(RequestTenant tenant);
        Task<IResult<ResponseTenant>> UpdateTenantAsync(RequestTenant tenant, string tenantId);
        Task<IResult<string>> DeleteTenantAsync(string tenantId);


        Task<IResult<ResponsePermissionTenant>> GetPermissionsAsync(string tenantId);
        Task<IResult<ResponsePermissionTenant>> UpdatePermissionsAsync(RequestPermissionTenant request, string tenantId);
    }
}
