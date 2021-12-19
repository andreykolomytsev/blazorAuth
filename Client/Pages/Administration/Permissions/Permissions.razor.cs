using MudBlazor;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Infrastructure.Models.Response;
using AuthClient.Client.Infrastructure.Managers.Identity.Permissions;

namespace AuthClient.Client.Pages.Administration.Permissions
{
    public partial class Permissions
    {
        private readonly List<BreadcrumbItem> _items = new()
        {
            new BreadcrumbItem("", href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem("Управление правами доступа", href: null, disabled: true),
        };

        [Inject] private IPermissionManager PermissionManager { get; set; }

        private bool _loading = true;

        private List<ResponsePermission> _permissionsList = new();
        private PaginatedResult<ResponsePermission> _responseData = new();
        private MudTable<ResponsePermission> _tableData;

        /// <summary>
        /// Загрузка данных в таблицу
        /// </summary>
        private async Task<TableData<ResponsePermission>> ServerReloadAsync(TableState state)
        {
            await GetDataAsync(state.Page + 1, state.PageSize);

            if (!_responseData.Succeeded) return new TableData<ResponsePermission>() { TotalItems = 0, Items = _permissionsList };

            _permissionsList = _responseData.Response;

            return new TableData<ResponsePermission>() { TotalItems = _responseData.TotalCount, Items = _permissionsList };
        }

        /// <summary>
        /// Получить данные из API
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private async Task<PaginatedResult<ResponsePermission>> GetDataAsync(int page, int pageSize)
        {
            _loading = true;
            StateHasChanged();

            _responseData = await PermissionManager.GetAllPermissionsAsync(page, pageSize);

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
        /// Модальное окно прав доступа
        /// </summary>
        /// <returns></returns>
        private async Task InvokeModal(string permissionId = null)
        {
            var parameters = new DialogParameters();
            string title = "Новое право доступа";

            if (!string.IsNullOrEmpty(permissionId))
            {
                title = "Редактирование";
                parameters.Add("PermissionId", permissionId);
            }

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<PermissionModal>(title, parameters, options);

            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await ReloadDataAsync();
            }
        }

        /// <summary>
        /// Модальное окно удаления
        /// </summary>
        /// <param name="permissionId"></param>
        /// <param name="permissionName"></param>
        /// <returns></returns>
        private async Task DeleteModal(string permissionId, string permissionName)
        {
            string deleteContent = $"Вы действительно хотите удалить право доступа {permissionName}?";

            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, permissionId)},
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>("Удаление прав доступа", parameters, options);

            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await PermissionManager.DeletePermissionAsync(permissionId);
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
    }
}
