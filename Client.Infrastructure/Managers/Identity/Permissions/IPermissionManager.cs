using System.Threading.Tasks;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Infrastructure.Models.Response;
using AuthClient.Client.Infrastructure.Models.Request;

namespace AuthClient.Client.Infrastructure.Managers.Identity.Permissions
{
    public interface IPermissionManager : IManager
    {
        Task<PaginatedResult<ResponsePermission>> GetAllPermissionsAsync(int pageNumber, int pageSize);

        Task<IResult<ResponsePermission>> GetPermissionByIdAsync(string permissionId);

        Task<IResult<ResponsePermission>> CreatePermissionAsync(RequestPermission permission);

        Task<IResult<ResponsePermission>> UpdatePermissionAsync(RequestPermission permission, string permissionId);

        Task<IResult<string>> DeletePermissionAsync(string permissionId);
    }
}
