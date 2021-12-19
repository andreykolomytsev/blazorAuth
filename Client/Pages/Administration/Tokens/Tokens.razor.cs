using MudBlazor;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Shared.Dialogs;
using AuthClient.Shared.Constants.Permission;
using AuthClient.Client.Infrastructure.Models.Response;
using AuthClient.Client.Infrastructure.Managers.Identity.Tokens;

namespace AuthClient.Client.Pages.Administration.Tokens
{
    public partial class Tokens
    {
        private readonly List<BreadcrumbItem> _items = new()
        {
            new BreadcrumbItem("", href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem("Управление токенами", href: null, disabled: true),
        };

        [Inject] private ITokenManager TokenManager { get; set; }

        private ClaimsPrincipal _currentUser;

        private bool _canEditTokens;

        private bool _loading = true;

        private List<ResponseAccessToken> _tokenList = new();
        private PaginatedResult<ResponseAccessToken> _responseData = new();
        private MudTable<ResponseAccessToken> _tableData;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canEditTokens = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Tokens.Edit)).Succeeded;
        }

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

        /// <summary>
        /// Модальное окно изменение статуса
        /// </summary>
        /// <param name="token"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        private async Task ChangeStatusModal(string token, bool status)
        {
            string deleteContent = status ? "Вы действительно хотите заблокировать токен доступа?" : "Вы действительно хотите разблокировать токен доступа?";
            var parameters = new DialogParameters
            {
                {nameof(ChangeStatus.ContentText), string.Format(deleteContent, token)},
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<ChangeStatus>("Изменение статуса", parameters, options);

            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await TokenManager.ChangeStatus(token, !status);
                if (response.Succeeded)
                {
                    await ReloadDataAsync();
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private void MoreInfoModal(string token)
        {
            var parameters = new DialogParameters
            {
                { "TitleText", "Подробная информация" },
                { "LabelText", "Токен доступа" },
                { "ContentText", token }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };

            _dialogService.Show<Info>("Инфо", parameters, options);
        }
    }
}
