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
using AuthClient.Client.Infrastructure.Managers.Identity.Roles;

namespace AuthClient.Client.Pages.Administration.Roles
{
    public partial class Roles
    {
        private readonly List<BreadcrumbItem> _items = new()
        {
            new BreadcrumbItem("", href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem("Управление ролями", href: null, disabled: true),
        };

        [Inject] private IRoleManager RoleManager { get; set; }
        [Inject] private IJSRuntime JS { get; set; }

        private ClaimsPrincipal _currentUser;

        private bool _isSuperAdmin;
        private bool _canCreateRoles;
        private bool _canEditRoles;
        private bool _canDeleteRoles;
        private bool _canViewRoleClaims;
        private bool _loading = true;

        private List<ResponseRole> _roleList = new();
        private PaginatedResult<ResponseRole> _responseData = new();
        private MudTable<ResponseRole> _tableData;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _isSuperAdmin = _currentUser.IsInRole(RoleConstants.SuperAdminRole);
            _canCreateRoles = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Roles.Create)).Succeeded;
            _canEditRoles = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Roles.Edit)).Succeeded;
            _canDeleteRoles = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Roles.Delete)).Succeeded;
            _canViewRoleClaims = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.RolePermissions.View)).Succeeded;
        }

        /// <summary>
        /// Удаление ролей
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        private async Task DeleteModal(string roleId, string roleName)
        {
            string deleteContent = $"Вы действительно хотите удалить роль {roleName}?";
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, roleId)},
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>("Удаление роли", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await RoleManager.DeleteRoleAsync(roleId);
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
        /// Создание / Редактирование ролей
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        private async Task InvokeModal(string roleId = null)
        {
            var parameters = new DialogParameters();
            string title = "Новая роль";

            if (!string.IsNullOrEmpty(roleId))
            {
                title = "Редактирование";
                parameters.Add("RoleId", roleId);
            }

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<RoleModal>(title, parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await ReloadDataAsync();
            }
        }

        /// <summary>
        /// Управление правами доступа
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        private async Task ManagePermissionsModal(string roleId)
        {
            var parameters = new DialogParameters
            {
                { "RoleId", roleId }
            };

            string title = "Права доступа";

            var isMobileDevice = await JS.InvokeAsync<bool>("isMobile");

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true, FullScreen = isMobileDevice };
            _dialogService.Show<RolePermissionModal>(title, parameters, options);
        }

        /// <summary>
        /// Загрузка данных в таблицу
        /// </summary>
        private async Task<TableData<ResponseRole>> ServerReloadAsync(TableState state)
        {
            await GetDataAsync(state.Page + 1, state.PageSize);

            if (!_responseData.Succeeded) return new TableData<ResponseRole>() { TotalItems = 0, Items = _roleList };

            _roleList = _responseData.Response;

            return new TableData<ResponseRole>() { TotalItems = _responseData.TotalCount, Items = _roleList };
        }

        /// <summary>
        /// Получить данные из API
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private async Task<PaginatedResult<ResponseRole>> GetDataAsync(int page, int pageSize)
        {
            _loading = true;
            StateHasChanged();

            _responseData = await RoleManager.GetAllRolesAsync(page, pageSize);

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