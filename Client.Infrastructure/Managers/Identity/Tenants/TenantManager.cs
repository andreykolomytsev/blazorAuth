using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Infrastructure.Extensions;
using AuthClient.Client.Infrastructure.Models.Request;
using AuthClient.Client.Infrastructure.Models.Response;

namespace AuthClient.Client.Infrastructure.Managers.Identity.Tenants
{
    public class TenantManager : ITenantManager
    {
        private readonly HttpClient _httpClient;

        public TenantManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region TENANTS
        public async Task<PaginatedResult<ResponseTenant>> GetAllTenantsAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetAsync(Routes.TenantEndpoints.GetAll + "?PageNumber=" + pageNumber + "&pageSize=" + pageSize);
            return await response.ToPaginatedResult<ResponseTenant>();
        }

        public async Task<IResult<ResponseTenant>> GetTenantByIdAsync(string tenantId)
        {
            var response = await _httpClient.GetAsync(Routes.TenantEndpoints.GetById(tenantId));
            return await response.ToResult<ResponseTenant>();
        }

        public async Task<IResult<ResponseTenant>> CreateTenantAsync(RequestTenant tenant)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.TenantEndpoints.Create, tenant);
            return await response.ToResult<ResponseTenant>();
        }

        public async Task<IResult<ResponseTenant>> UpdateTenantAsync(RequestTenant tenant, string tenantId)
        {
            var response = await _httpClient.PutAsJsonAsync(Routes.TenantEndpoints.GetById(tenantId), tenant);
            return await response.ToResult<ResponseTenant>();
        }

        public async Task<IResult<string>> DeleteTenantAsync(string tenantId)
        {
            var response = await _httpClient.DeleteAsync(Routes.TenantEndpoints.GetById(tenantId));
            return await response.ToResult<string>();
        }
        #endregion

        #region PERMISSIONS
        public async Task<IResult<ResponsePermissionTenant>> GetPermissionsAsync(string tenantId)
        {
            var response = await _httpClient.GetAsync(Routes.TenantEndpoints.GetPermissionsById(tenantId));
            return await response.ToResult<ResponsePermissionTenant>();
        }

        public async Task<IResult<ResponsePermissionTenant>> UpdatePermissionsAsync(RequestPermissionTenant request, string tenantId)
        {
            var response = await _httpClient.PutAsJsonAsync(Routes.TenantEndpoints.GetPermissionsById(tenantId), request);
            return await response.ToResult<ResponsePermissionTenant>();
        }
        #endregion
    }
}
