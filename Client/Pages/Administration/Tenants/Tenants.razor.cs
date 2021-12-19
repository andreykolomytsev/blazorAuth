using MudBlazor;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using AuthClient.Shared.Wrapper;
using AuthClient.Shared.Constants.Role;
using AuthClient.Shared.Constants.Permission;
using AuthClient.Client.Infrastructure.Models.Response;
using AuthClient.Client.Infrastructure.Managers.Identity.Tenants;

namespace AuthClient.Client.Pages.Administration.Tenants
{
    public partial class Tenants
    {
        private readonly List<BreadcrumbItem> _items = new()
        {
            new BreadcrumbItem("", href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem("Управление организациями", href: null, disabled: true),
        };

        [Inject] private ITenantManager TenantManager { get; set; }
        [Inject] private IJSRuntime JS { get; set; }

        private ClaimsPrincipal _currentUser;

        private bool _canCreateTenants;
        private bool _canEditTenants;
        private bool _canDeleteTenants;
        private bool _canEditPermissionTenants;
        private bool _loading = true;

        private List<ResponseTenant> _tenantList = new();
        private PaginatedResult<ResponseTenant> _responseData = new();
        private MudTable<ResponseTenant> _tableData;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateTenants = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Tenants.Create)).Succeeded;
            _canEditTenants = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Tenants.Edit)).Succeeded;
            _canDeleteTenants = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Tenants.Delete)).Succeeded;
            _canEditPermissionTenants = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.TenantPermissions.View)).Succeeded;
        }


        /// <summary>
        /// Загрузка данных в таблицу
        /// </summary>
        private async Task<TableData<ResponseTenant>> ServerReloadAsync(TableState state)
        {
            await GetDataAsync(state.Page + 1, state.PageSize);

            if (!_responseData.Succeeded) return new TableData<ResponseTenant>() { TotalItems = 0, Items = _tenantList };

            _tenantList = _responseData.Response;

            return new TableData<ResponseTenant>() { TotalItems = _responseData.TotalCount, Items = _tenantList };
        }

        private TableGroupDefinition<ResponseTenant> _groupDefinition = new TableGroupDefinition<ResponseTenant>()
        {
            GroupName = "Записано на организацию",
            Indentation = false,
            Expandable = true,
            IsInitiallyExpanded = false,
            Selector = (e) => e.Tenant?.Id,
        };

        /// <summary>
        /// Получить данные из API
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private async Task<PaginatedResult<ResponseTenant>> GetDataAsync(int page, int pageSize)
        {
            _loading = true;
            StateHasChanged();

            _responseData = await TenantManager.GetAllTenantsAsync(page, pageSize);

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
        /// Модальное окно организации
        /// </summary>
        /// <returns></returns>
        private async Task InvokeModal(string tenantId = null)
        {
            var parameters = new DialogParameters();
            string title = "Новая организация";

            if (!string.IsNullOrEmpty(tenantId))
            {
                title = "Редактирование";
                parameters.Add("TenantId", tenantId);
            }

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<TenantModal>(title, parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await ReloadDataAsync();
            }
        }

        /// <summary>
        /// Модальное окно удаления
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="tenantName"></param>
        /// <returns></returns>
        private async Task DeleteModal(string tenantId, string tenantName)
        {
            string deleteContent = $"Вы действительно хотите удалить организацию {tenantName}?";
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, tenantId)},
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>("Удаление организации", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await TenantManager.DeleteTenantAsync(tenantId);
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
        /// Управление правами доступа
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        private async Task ManagePermissionsModal(string tenantId)
        {
            var parameters = new DialogParameters
            {
                { "TenantId", tenantId }
            };

            string title = "Права доступа";

            var isMobileDevice = await JS.InvokeAsync<bool>("isMobile");

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true, FullScreen = isMobileDevice };
            _dialogService.Show<TenantPermissionModal>(title, parameters, options);
        }
    }
}
