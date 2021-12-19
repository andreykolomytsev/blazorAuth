using System.Threading.Tasks;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Infrastructure.Parameters;
using AuthClient.Client.Infrastructure.Models.Request;
using AuthClient.Client.Infrastructure.Models.Response;

namespace AuthClient.Client.Infrastructure.Managers.Identity.Users
{
    public interface IUserManager : IManager
    {
        Task<PaginatedResult<ResponseUser>> GetAllUsersAsync(int pageNumber, int pageSize, UserFilter filter = null);
        Task<PaginatedResult<ResponseUser>> GetUsersByParentUser(int pageNumber, int pageSize, string userId, UserFilter filter = null);
        Task<IResult<ResponseUser>> GetUserByIdAsync(string userId);
        Task<IResult<ResponseUser>> CreateUserAsync(RequestUser request);
        Task<IResult<ResponseUser>> UpdateUserAsync(RequestUserUpdate request, string userId);


        Task<IResult<ResponseUserRoles>> GetUserRolesAsync(string userId);
        Task<IResult<ResponseUserRoles>> UpdateUserRolesAsync(RequestUpdateUserRoles request);
    }
}