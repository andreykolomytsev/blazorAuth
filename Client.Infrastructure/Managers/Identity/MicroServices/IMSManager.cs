using System.Threading.Tasks;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Infrastructure.Models.Request;
using AuthClient.Client.Infrastructure.Models.Response;

namespace AuthClient.Client.Infrastructure.Managers.Identity.MicroServices
{
    public interface IMSManager : IManager
    {
        Task<PaginatedResult<ResponseMS>> GetAllMSsAsync(int pageNumber, int pageSize);
        Task<IResult<ResponseMS>> GetMSByIdAsync(string msId);
        Task<IResult<ResponseMS>> CreateMSAsync(RequestMS ms);
        Task<IResult<ResponseMS>> UpdateMSAsync(RequestMS ms, string msId);
        Task<IResult<string>> DeleteMSAsync(string msId);


        Task<IResult<ResponseServiceUser>> GetAllUsersByService(string msId);
        Task<IResult<ResponseServiceUser>> UpdateUsersByService(RequestServiceUser requestServiceUser, string msId);

        Task<IResult<ResponseServiceTenant>> GetAllTenantsByService(string msId);
        Task<IResult<ResponseServiceTenant>> UpdateTenantsByService(RequestServiceTenant requestServiceTenant, string msId);
    }
}
