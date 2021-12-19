using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Infrastructure.Extensions;
using AuthClient.Client.Infrastructure.Models.Request;
using AuthClient.Client.Infrastructure.Models.Response;

namespace AuthClient.Client.Infrastructure.Managers.Identity.MicroServices
{
    public class MSManager : IMSManager
    {
        private readonly HttpClient _httpClient;

        public MSManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region SERVICES
        public async Task<PaginatedResult<ResponseMS>> GetAllMSsAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetAsync(Routes.ServiceEndpoints.GetAll + "?PageNumber=" + pageNumber + "&pageSize=" + pageSize);
            return await response.ToPaginatedResult<ResponseMS>();
        }

        public async Task<IResult<ResponseMS>> GetMSByIdAsync(string msId)
        {
            var response = await _httpClient.GetAsync(Routes.ServiceEndpoints.GetById(msId));
            return await response.ToResult<ResponseMS>();
        }

        public async Task<IResult<ResponseMS>> CreateMSAsync(RequestMS ms)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ServiceEndpoints.Create, ms);
            return await response.ToResult<ResponseMS>();
        }

        public async Task<IResult<ResponseMS>> UpdateMSAsync(RequestMS ms, string msId)
        {
            var response = await _httpClient.PutAsJsonAsync(Routes.ServiceEndpoints.GetById(msId), ms);
            return await response.ToResult<ResponseMS>();
        }

        public async Task<IResult<string>> DeleteMSAsync(string msId)
        {
            var response = await _httpClient.DeleteAsync(Routes.ServiceEndpoints.GetById(msId));
            return await response.ToResult<string>();
        }
        #endregion

        #region USER-SERVICES
        public async Task<IResult<ResponseServiceUser>> GetAllUsersByService(string msId)
        {
            var response = await _httpClient.GetAsync(Routes.ServiceEndpoints.GetUserById(msId));
            return await response.ToResult<ResponseServiceUser>();
        }

        public async Task<IResult<ResponseServiceUser>> UpdateUsersByService(RequestServiceUser requestServiceUser, string msId)
        {
            var response = await _httpClient.PutAsJsonAsync(Routes.ServiceEndpoints.GetUserById(msId), requestServiceUser);
            return await response.ToResult<ResponseServiceUser>();
        }
        #endregion

        #region TENANT-SERVICES
        public async Task<IResult<ResponseServiceTenant>> GetAllTenantsByService(string msId)
        {
            var response = await _httpClient.GetAsync(Routes.ServiceEndpoints.GetTenantById(msId));
            return await response.ToResult<ResponseServiceTenant>();
        }

        public async Task<IResult<ResponseServiceTenant>> UpdateTenantsByService(RequestServiceTenant requestServiceTenant, string msId)
        {
            var response = await _httpClient.PutAsJsonAsync(Routes.ServiceEndpoints.GetTenantById(msId), requestServiceTenant);
            return await response.ToResult<ResponseServiceTenant>();
        }
        #endregion
    }
}
