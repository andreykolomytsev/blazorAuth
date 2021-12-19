using MudBlazor;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using AuthClient.Shared.Constants.Permission;
using AuthClient.Client.Infrastructure.Models.Request;
using AuthClient.Client.Infrastructure.Models.Response;
using AuthClient.Client.Infrastructure.Managers.Identity.Roles;

namespace AuthClient.Client.Pages.Administration.Roles
{
    public partial class RolePermissionModal
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        [Inject] private IRoleManager RoleManager { get; set; }
        [Parameter] public string RoleId { get; set; }

        private ResponsePermissionRole _rolePermissionList = new();
        private Dictionary<string, List<ResponsePermission>> GroupedPermissions { get; } = new();

        private ClaimsPrincipal _currentUser;
        private bool _canEditRolePermissions;
        private bool _loading = true;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canEditRolePermissions = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.RolePermissions.Edit)).Succeeded;

            await GetPermissionsAsync();
        }

        private async Task GetPermissionsAsync()
        {
            _loading = true;
            StateHasChanged();

            var result = await RoleManager.GetPermissionsAsync(RoleId);

            if (result.Succeeded)
            {
                _rolePermissionList = result.Response;

                foreach (var permission in _rolePermissionList.Permissions)
                {
                    var name = permission.Value.Split(new char[] { '.' })[2];
                    var group = permission.Value.Split(new char[] { '.' })[1];

                    if (group.Contains("Users")) group = "Пользователи";
                    if (group.Contains("Roles")) group = "Роли";
                    if (group == "Permissions") group = "Права доступа";
                    if (group.Contains("RolePermissions")) group = "ПД к ролям";
                    if (group.Contains("TenantPermissions")) group = "ПД к организациям";
                    if (group.Contains("Tokens")) group = "Токены";
                    if (group.Contains("Services")) group = "Сервисы";
                    if (group.Contains("Tenants")) group = "Организации";

                    if (name.Contains("View")) permission.Name = "Просмотр";
                    if (name.Contains("Create")) permission.Name = "Создание";
                    if (name.Contains("Edit")) permission.Name = "Редактирование";
                    if (name.Contains("Delete")) permission.Name = "Удаление";
                    if (name.Contains("Search")) permission.Name = "Поиск";
                    if (name.Contains("Revoke")) permission.Name = "Отозвать";

                    if (GroupedPermissions.ContainsKey(group))
                    {
                        GroupedPermissions[group].Add(permission);
                    }
                    else
                    {
                        GroupedPermissions.Add(group, new List<ResponsePermission> { permission });
                    }
                }

                MudDialog.SetTitle($"Права доступа для {_rolePermissionList.RoleName}");
            }
            else
            {
                foreach (var error in result.Messages)
                {
                    _snackBar.Add(error, Severity.Error);
                }
            }

            _loading = false;
            StateHasChanged();
        }

        /// <summary>
        /// Отправка формы
        /// </summary>
        /// <returns></returns>
        private async Task SaveAsync()
        {
            _loading = true;

            var requestModel = new RequestPermissionRole();

            foreach (var group in GroupedPermissions.Keys)
            {
                foreach (var permission in GroupedPermissions[group].Where(x => x.Selected).ToList())
                {
                    requestModel.PermissionRoles.Add(new PermissionRole() { PermissionId = permission.Id });
                }
            }

            var result = await RoleManager.UpdatePermissionsAsync(requestModel, RoleId);

            if (result.Succeeded)
            {
                _snackBar.Add(result.Messages[0], Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
            else
            {
                foreach (var error in result.Messages)
                {
                    _snackBar.Add(error, Severity.Error);
                }
            }

            _loading = false;
        }

        /// <summary>
        /// Цвет бейджа
        /// </summary>
        /// <param name="selected"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        private Color GetGroupBadgeColor(int selected, int all)
        {
            if (selected == 0)
                return Color.Error;

            if (selected == all)
                return Color.Success;

            return Color.Info;
        }

        /// <summary>
        /// Закрыть текущее модальное окно
        /// </summary>
        private void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
