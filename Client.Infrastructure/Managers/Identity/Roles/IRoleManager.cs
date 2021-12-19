using System.Threading.Tasks;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Infrastructure.Models.Request;
using AuthClient.Client.Infrastructure.Models.Response;

namespace AuthClient.Client.Infrastructure.Managers.Identity.Roles
{
    public interface IRoleManager : IManager
    {
        Task<PaginatedResult<ResponseRole>> GetAllRolesAsync(int pageNumber, int pageSize);
        Task<IResult<ResponseRole>> GetRoleByIdAsync(string roleId);
        Task<IResult<ResponseRole>> CreateRoleAsync(RequestRole role);
        Task<IResult<ResponseRole>> UpdateRoleAsync(RequestRole role, string roleId);
        Task<IResult<string>> DeleteRoleAsync(string roleId);


        Task<IResult<ResponsePermissionRole>> GetPermissionsAsync(string roleId);
        Task<IResult<ResponsePermissionRole>> UpdatePermissionsAsync(RequestPermissionRole request, string roleId);
    }
}