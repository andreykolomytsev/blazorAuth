using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AuthClient.Client.Infrastructure.Extensions;
using AuthClient.Client.Infrastructure.Models.Request;
using AuthClient.Client.Infrastructure.Models.Response;
using AuthClient.Shared.Wrapper;

namespace AuthClient.Client.Infrastructure.Managers.Identity.Permissions
{
    public class PermissionManager : IPermissionManager
    {
        private readonly HttpClient _httpClient;

        public PermissionManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedResult<ResponsePermission>> GetAllPermissionsAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetAsync(Routes.PermissionEndpoints.GetAll + "?PageNumber=" + pageNumber + "&pageSize=" + pageSize);
            return await response.ToPaginatedResult<ResponsePermission>();
        }

        public async Task<IResult<ResponsePermission>> GetPermissionByIdAsync(string permissionId)
        {
            var response = await _httpClient.GetAsync(Routes.PermissionEndpoints.GetById(permissionId));
            return await response.ToResult<ResponsePermission>();
        }

        public async Task<IResult<ResponsePermission>> CreatePermissionAsync(RequestPermission permission)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.PermissionEndpoints.Create, permission);
            return await response.ToResult<ResponsePermission>();
        }

        public async Task<IResult<ResponsePermission>> UpdatePermissionAsync(RequestPermission permission, string permissionId)
        {
            var response = await _httpClient.PutAsJsonAsync(Routes.PermissionEndpoints.GetById(permissionId), permission);
            return await response.ToResult<ResponsePermission>();
        }

        public async Task<IResult<string>> DeletePermissionAsync(string permissionId)
        {
            var response = await _httpClient.DeleteAsync(Routes.PermissionEndpoints.GetById(permissionId));
            return await response.ToResult<string>();
        }
    }
}
