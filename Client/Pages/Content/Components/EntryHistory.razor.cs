using MudBlazor;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Extensions;
using AuthClient.Client.Infrastructure.Parameters;
using AuthClient.Client.Infrastructure.Models.Response;
using AuthClient.Client.Infrastructure.Managers.Identity.Tokens;

namespace AuthClient.Client.Pages.Content.Components
{
    public partial class EntryHistory
    {
        [Inject] private ITokenManager TokenManager { get; set; }

        private bool _loading = true;

        private List<ResponseAccessToken> _tokenList = new();
        private PaginatedResult<ResponseAccessToken> _responseData = new();

        /// <summary>
        /// Загрузка данных в таблицу
        /// </summary>
        private async Task<TableData<ResponseAccessToken>> ServerReloadAsync(TableState state)
        {
            await GetDataAsync(state.Page + 1, state.PageSize);

            if (!_responseData.Succeeded) return new TableData<ResponseAccessToken>() { TotalItems = 0, Items = _tokenList };

            _tokenList = _responseData.Response;

            return new TableData<ResponseAccessToken>() { TotalItems = _responseData.TotalCount, Items = _tokenList };
        }

        /// <summary>
        /// Получить данные из API
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private async Task<PaginatedResult<ResponseAccessToken>> GetDataAsync(int page, int pageSize)
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();

            _loading = true;
            StateHasChanged();

            _responseData = await TokenManager.GetAccessTokensAsync(page, pageSize, new TokenFilter() { UserId = state.User.GetUserId() });

            _loading = false;
            StateHasChanged();

            return _responseData;
        }
    }
}
