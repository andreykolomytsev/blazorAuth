using MudBlazor;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using AuthClient.Shared.Wrapper;
using AuthClient.Shared.Constants.Permission;
using AuthClient.Client.Infrastructure.Models.Response;
using AuthClient.Client.Infrastructure.Managers.Identity.MicroServices;

namespace AuthClient.Client.Pages.Administration.MicroServices
{
    public partial class MicroServices
    {
        private readonly List<BreadcrumbItem> _items = new()
        {
            new BreadcrumbItem("", href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem("Управление микросервисами", href: null, disabled: true),
        };

        [Inject] private IJSRuntime JS { get; set; }
        [Inject] private IMSManager MSManager { get; set; }

        private ClaimsPrincipal _currentUser;

        private bool _canCreateMS;
        private bool _canEditMS;
        private bool _canDeleteMS;
        private bool _loading = true;

        private List<ResponseMS> _msList = new();
        private PaginatedResult<ResponseMS> _responseData = new();
        private MudTable<ResponseMS> _tableData = null;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateMS = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Services.Create)).Succeeded;
            _canEditMS = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Services.Edit)).Succeeded;
            _canDeleteMS = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Services.Delete)).Succeeded;
        }


        /// <summary>
        /// Загрузка данных в таблицу
        /// </summary>
        private async Task<TableData<ResponseMS>> ServerReloadAsync(TableState state)
        {
            await GetDataAsync(state.Page + 1, state.PageSize);

            if (!_responseData.Succeeded) return new TableData<ResponseMS>() { TotalItems = 0, Items = _msList };

            _msList = _responseData.Response;

            return new TableData<ResponseMS>() { TotalItems = _responseData.TotalCount, Items = _msList };
        }

        /// <summary>
        /// Получить данные из API
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private async Task<PaginatedResult<ResponseMS>> GetDataAsync(int page, int pageSize)
        {
            _loading = true;
            StateHasChanged();

            _responseData = await MSManager.GetAllMSsAsync(page, pageSize);

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
        /// Модальное окно микросервиса
        /// </summary>
        /// <returns></returns>
        private async Task InvokeModal(string msId = null)
        {
            var parameters = new DialogParameters();
            string title = "Новый сервис";

            if (!string.IsNullOrEmpty(msId))
            {
                title = "Редактирование";
                parameters.Add("MSId", msId);
            }

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<MicroServicesModal>(title, parameters, options);

            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await ReloadDataAsync();
            }
        }

        /// <summary>
        /// Модальное окно удаления
        /// </summary>
        /// <param name="msId"></param>
        /// <param name="msName"></param>
        /// <returns></returns>
        private async Task DeleteModal(string msId, string msName)
        {
            string deleteContent = $"Вы действительно хотите удалить сервис {msName}?";
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, msId)},
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>("Удаление сервиса", parameters, options);

            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await MSManager.DeleteMSAsync(msId);

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

        /// <summary>
        /// Управление пользователями микросервисов
        /// </summary>
        /// <returns></returns>
        private async Task ManageUsers(string msId = null)
        {
            var isMobileDevice = await JS.InvokeAsync<bool>("isMobile");

            var parameters = new DialogParameters
            {
                { "MSId", msId }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Large, FullWidth = true, DisableBackdropClick = true, FullScreen = isMobileDevice };
            _dialogService.Show<MicroServiceUsersModal>("Настройка пользователей для", parameters, options);
        }

        /// <summary>
        /// Управление организациями микросервисов
        /// </summary>
        /// <returns></returns>
        private async Task ManageTenants(string msId = null)
        {
            var isMobileDevice = await JS.InvokeAsync<bool>("isMobile");

            var parameters = new DialogParameters
            {
                { "MSId", msId }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Large, FullWidth = true, DisableBackdropClick = true, FullScreen = isMobileDevice };
            _dialogService.Show<MicroServiceTenantsModal>("Настройка организаций для", parameters, options);
        }
    }
}
