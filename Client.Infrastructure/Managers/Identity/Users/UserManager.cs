using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Infrastructure.Parameters;
using AuthClient.Client.Infrastructure.Extensions;
using AuthClient.Client.Infrastructure.Models.Request;
using AuthClient.Client.Infrastructure.Models.Response;

namespace AuthClient.Client.Infrastructure.Managers.Identity.Users
{
    public class UserManager : IUserManager
    {
        private readonly HttpClient _httpClient;

        public UserManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region USERS
        public async Task<PaginatedResult<ResponseUser>> GetAllUsersAsync(int pageNumber, int pageSize, UserFilter filter = null)
        {
            var queryString = Routes.UserEndpoints.GetAll + "?PageNumber=" + pageNumber + "&pageSize=" + pageSize;

            if (filter is not null)
            {
                if (!string.IsNullOrEmpty(filter.FirstName))
                    queryString += "&FirstName=" + filter.FirstName;

                if (!string.IsNullOrEmpty(filter.LastName))
                    queryString += "&LastName=" + filter.LastName;

                if (!string.IsNullOrEmpty(filter.MiddleName))
                    queryString += "&MiddleName=" + filter.MiddleName;

                if (!string.IsNullOrEmpty(filter.Email))
                    queryString += "&Email=" + filter.Email;

                if (!string.IsNullOrEmpty(filter.PhoneNumber))
                    queryString += "&PhoneNumber=" + filter.PhoneNumber;
            }


            var response = await _httpClient.GetAsync(queryString);
            return await response.ToPaginatedResult<ResponseUser>();
        }

        public async Task<PaginatedResult<ResponseUser>> GetUsersByParentUser(int pageNumber, int pageSize, string userId, UserFilter filter = null)
        {
            var queryString = Routes.UserEndpoints.GetUsersByParentUser(userId) + "?PageNumber=" + pageNumber + "&pageSize=" + pageSize;

            if (filter is not null)
            {
                if (!string.IsNullOrEmpty(filter.FirstName))
                    queryString += "&FirstName=" + filter.FirstName;

                if (!string.IsNullOrEmpty(filter.LastName))
                    queryString += "&LastName=" + filter.LastName;

                if (!string.IsNullOrEmpty(filter.MiddleName))
                    queryString += "&MiddleName=" + filter.MiddleName;

                if (!string.IsNullOrEmpty(filter.Email))
                    queryString += "&Email=" + filter.Email;

                if (!string.IsNullOrEmpty(filter.PhoneNumber))
                    queryString += "&PhoneNumber=" + filter.PhoneNumber;
            }


            var response = await _httpClient.GetAsync(queryString);
            return await response.ToPaginatedResult<ResponseUser>();
        }

        public async Task<IResult<ResponseUser>> GetUserByIdAsync(string userId)
        {
            var response = await _httpClient.GetAsync(Routes.UserEndpoints.GetById(userId));
            return await response.ToResult<ResponseUser>();
        }

        public async Task<IResult<ResponseUser>> CreateUserAsync(RequestUser request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.UserEndpoints.Create, request);
            return await response.ToResult<ResponseUser>();
        }

        public async Task<IResult<ResponseUser>> UpdateUserAsync(RequestUserUpdate request, string userId)
        {
            var response = await _httpClient.PutAsJsonAsync(Routes.UserEndpoints.GetById(userId), request);
            return await response.ToResult<ResponseUser>();
        }
        #endregion

        #region ROLES
        public async Task<IResult<ResponseUserRoles>> GetUserRolesAsync(string userId)
        {
            var response = await _httpClient.GetAsync(Routes.UserEndpoints.GetUserRoles(userId));
            return await response.ToResult<ResponseUserRoles>();
        }

        public async Task<IResult<ResponseUserRoles>> UpdateUserRolesAsync(RequestUpdateUserRoles request)
        {
            var response = await _httpClient.PutAsJsonAsync(Routes.UserEndpoints.GetUserRoles(request.UserId), request);
            return await response.ToResult<ResponseUserRoles>();
        }
        #endregion
    }
}