using MudBlazor;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using AuthClient.Shared.Wrapper;
using AuthClient.Shared.Constants.Permission;
using AuthClient.Client.Infrastructure.Models.Response;
using System.Linq;

namespace AuthClient.Client.Pages.Administration.Users
{
    public partial class Users
    {
        private readonly List<BreadcrumbItem> _items = new()
        {
            new BreadcrumbItem("", href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem("Управление пользователями", href: null, disabled: true),
        };

        [Inject] private IJSRuntime JS { get; set; }

        private ClaimsPrincipal _currentUser;

        private bool _canCreateUsers;
        private bool _canEditUsers;
        private bool _canViewRoles;
        private bool _loading = true;
        private bool _isMobileDevice = false;
        private bool _dense = false;

        private List<ResponseUser> _userList = new();
        private PaginatedResult<ResponseUser> _responseData = new();
        private MudTable<ResponseUser> _tableData;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateUsers = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Users.Create)).Succeeded;
            _canEditUsers = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Users.Edit)).Succeeded;
            _canViewRoles = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Roles.View)).Succeeded;

            _isMobileDevice = await JS.InvokeAsync<bool>("isMobile");
        }

        /// <summary>
        /// Загрузка данных в таблицу
        /// </summary>
        private async Task<TableData<ResponseUser>> ServerReloadAsync(TableState state)
        {
            await GetDataAsync(state.Page + 1, state.PageSize);

            if (!_responseData.Succeeded) return new TableData<ResponseUser>() { TotalItems = 0, Items = _userList };

            _userList = _responseData.Response;

            return new TableData<ResponseUser>() { TotalItems = _responseData.TotalCount, Items = _userList };
        }

        /// <summary>
        /// Получить данные из API
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private async Task<PaginatedResult<ResponseUser>> GetDataAsync(int page, int pageSize)
        {
            _loading = true;
            StateHasChanged();

            _responseData = await _userManager.GetAllUsersAsync(page, pageSize);

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
        /// Модальное окно пользователя
        /// </summary>
        /// <returns></returns>
        private async Task InvokeModal(string userId = null)
        {
            var parameters = new DialogParameters();
            string title = "Новый пользователь";

            if (!string.IsNullOrEmpty(userId))
            {
                title = "Редактирование";
                parameters.Add("UserId", userId);
            }

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<UserModal>(title, parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await ReloadDataAsync();
            }
        }

        private void ManageRoles(string userId, string email, ResponseUser user)
        {
            if (email == "admin@gmail.com") _snackBar.Add("Запрещено", Severity.Error);
            else
            {
                var parameters = new DialogParameters
                {
                    { "UserId", userId },
                    { "User", user.FirstName + " " + user.LastName }
                };

                var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true, FullScreen = _isMobileDevice };
                _dialogService.Show<UserRolesModal>("Настройка ролей для", parameters, options);
            }
        }

        private void ChangeDense(bool dense)
        {
            _dense = dense;
        }
    }
}