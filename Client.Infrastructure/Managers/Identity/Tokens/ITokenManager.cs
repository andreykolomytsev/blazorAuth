using System.Threading.Tasks;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Infrastructure.Parameters;
using AuthClient.Client.Infrastructure.Models.Response;

namespace AuthClient.Client.Infrastructure.Managers.Identity.Tokens
{
    public interface ITokenManager : IManager
    {
        Task<PaginatedResult<ResponseAccessToken>> GetAccessTokensAsync(int pageNumber, int pageSize, TokenFilter filter = null);

        Task<IResult<string>> ChangeStatus(string token, bool status);
    }
}
