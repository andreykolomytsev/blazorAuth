using MudBlazor;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using AuthClient.Client.Infrastructure.Models.Request;
using AuthClient.Client.Infrastructure.Models.Response;
using AuthClient.Client.Infrastructure.Managers.Identity.Tenants;

namespace AuthClient.Client.Pages.Administration.Tenants
{
    public partial class TenantPermissionModal
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        [Inject] private ITenantManager TenantManager { get; set; }
        [Parameter] public string TenantId { get; set; }

        private ResponsePermissionTenant _tenantPermissionList = new();
        private Dictionary<string, List<ResponsePermission>> GroupedPermissions { get; } = new();

        private bool _loading = false;

        protected override async Task OnInitializedAsync()
        {
            await GetPermissionsAsync();
        }

        /// <summary>
        /// Получить права доступа
        /// </summary>
        /// <returns></returns>
        private async Task GetPermissionsAsync()
        {
            _loading = true;
            StateHasChanged();

            var result = await TenantManager.GetPermissionsAsync(TenantId);

            if (result.Succeeded)
            {
                _tenantPermissionList = result.Response;

                foreach (var permission in _tenantPermissionList.Permissions)
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

                MudDialog.SetTitle($"Права доступа для {_tenantPermissionList.TenantName}");
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

            var requestModel = new RequestPermissionTenant();

            foreach (var group in GroupedPermissions.Keys)
            {
                foreach (var permission in GroupedPermissions[group].Where(x => x.Selected).ToList())
                {
                    requestModel.PermissionTenants.Add(new PermissionTenant() { PermissionId = permission.Id });
                }
            }

            var result = await TenantManager.UpdatePermissionsAsync(requestModel, TenantId);

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
