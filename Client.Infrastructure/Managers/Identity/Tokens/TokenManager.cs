using System.Net.Http;
using System.Threading.Tasks;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Infrastructure.Extensions;
using AuthClient.Client.Infrastructure.Models.Response;
using AuthClient.Client.Infrastructure.Parameters;

namespace AuthClient.Client.Infrastructure.Managers.Identity.Tokens
{
    public class TokenManager : ITokenManager
    {
        private readonly HttpClient _httpClient;

        public TokenManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedResult<ResponseAccessToken>> GetAccessTokensAsync(int pageNumber, int pageSize, TokenFilter filter = null)
        {
            var queryString = Routes.TokenEndpoints.GetAccessTokens + "?PageNumber=" + pageNumber + "&pageSize=" + pageSize;

            if (filter is not null)
            {
                if (!string.IsNullOrEmpty(filter.Token))
                    queryString += "&Token=" + filter.Token;

                if (filter.Created is not null)
                    queryString += "&Created=" + filter.Created;

                if (!string.IsNullOrEmpty(filter.CreatedByBrowser))
                    queryString += "&CreatedByBrowser=" + filter.CreatedByBrowser;

                if (!string.IsNullOrEmpty(filter.CreatedByIp))
                    queryString += "&CreatedByIp=" + filter.CreatedByIp;

                if (filter.Expires is not null)
                    queryString += "&Expires=" + filter.Expires;

                if (filter.IsActive is not null)
                    queryString += "&IsActive=" + filter.IsActive;

                if (!string.IsNullOrEmpty(filter.UserId))
                    queryString += "&UserId=" + filter.UserId;
            }

            var response = await _httpClient.GetAsync(queryString);
            return await response.ToPaginatedResult<ResponseAccessToken>();
        }

        public async Task<IResult<string>> ChangeStatus(string token, bool status)
        {
            var response = await _httpClient.PatchAsync(Routes.TokenEndpoints.ChangeStatus + "?Token=" + token + "&active=" + status, null);
            return await response.ToResult<string>();
        }
    }
}
