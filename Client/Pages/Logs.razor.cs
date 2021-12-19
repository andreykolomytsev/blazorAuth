using MudBlazor;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Infrastructure.Models.Response;
using AuthClient.Client.Infrastructure.Managers.Identity.Tokens;

namespace AuthClient.Client.Pages
{
    public partial class Logs
    {
        private readonly List<BreadcrumbItem> _items = new()
        {
            new BreadcrumbItem("", href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem("История входов пользователей", href: null, disabled: true),
        };

        [Inject] private ITokenManager TokenManager { get; set; }

        private bool _loading = true;

        private List<ResponseAccessToken> _tokenList = new();
        private PaginatedResult<ResponseAccessToken> _responseData = new();
        private MudTable<ResponseAccessToken> _tableData;

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
            _loading = true;
            StateHasChanged();

            _responseData = await TokenManager.GetAccessTokensAsync(page, pageSize);

            _loading = false;
            StateHasChanged();

            return _responseData;
        }

        /// <summary>
        /// Обновить данные
        /// </summary>
        /// <returns></returns>
        private async Task ReloadDataAsync()
        {
            await _tableData.ReloadServerData();
        }
    }
}
