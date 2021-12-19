using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Infrastructure.Extensions;
using AuthClient.Client.Infrastructure.Models.Request;
using AuthClient.Client.Infrastructure.Models.Response;

namespace AuthClient.Client.Infrastructure.Managers.Identity.Roles
{
    public class RoleManager : IRoleManager
    {
        private readonly HttpClient _httpClient;

        public RoleManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region ROLES
        public async Task<PaginatedResult<ResponseRole>> GetAllRolesAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetAsync(Routes.RolesEndpoints.GetAll + "?PageNumber=" + pageNumber + "&pageSize=" + pageSize);
            return await response.ToPaginatedResult<ResponseRole>();
        }

        public async Task<IResult<ResponseRole>> GetRoleByIdAsync(string roleId)
        {
            var response = await _httpClient.GetAsync(Routes.RolesEndpoints.GetById(roleId));
            return await response.ToResult<ResponseRole>();
        }

        public async Task<IResult<ResponseRole>> CreateRoleAsync(RequestRole role)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.RolesEndpoints.Create, role);
            return await response.ToResult<ResponseRole>();
        }

        public async Task<IResult<ResponseRole>> UpdateRoleAsync(RequestRole role, string roleId)
        {
            var response = await _httpClient.PutAsJsonAsync(Routes.RolesEndpoints.GetById(roleId), role);
            return await response.ToResult<ResponseRole>();
        }

        public async Task<IResult<string>> DeleteRoleAsync(string roleId)
        {
            var response = await _httpClient.DeleteAsync(Routes.RolesEndpoints.GetById(roleId));
            return await response.ToResult<string>();
        }
        #endregion

        #region PERMISSIONS
        public async Task<IResult<ResponsePermissionRole>> GetPermissionsAsync(string roleId)
        {
            var response = await _httpClient.GetAsync(Routes.RolesEndpoints.GetPermissionsById(roleId));
            return await response.ToResult<ResponsePermissionRole>();
        }

        public async Task<IResult<ResponsePermissionRole>> UpdatePermissionsAsync(RequestPermissionRole request, string roleId)
        {
            var response = await _httpClient.PutAsJsonAsync(Routes.RolesEndpoints.GetPermissionsById(roleId), request);
            return await response.ToResult<ResponsePermissionRole>();
        }
        #endregion
    }
}