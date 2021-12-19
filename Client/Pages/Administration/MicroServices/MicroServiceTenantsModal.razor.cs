using MudBlazor;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using AuthClient.Shared.Constants.Permission;
using AuthClient.Client.Infrastructure.Models.Response;
using AuthClient.Client.Infrastructure.Models.Request;
using AuthClient.Client.Infrastructure.Managers.Identity.MicroServices;

namespace AuthClient.Client.Pages.Administration.MicroServices
{
    public partial class MicroServiceTenantsModal
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        [Inject] private IMSManager MSManager { get; set; }

        [Parameter] public string MSId { get; set; }

        private List<ResponseTenantService> _serviceTenantList = new();

        private string _searchString = "";

        private ClaimsPrincipal _currentUser;

        private bool _canEditMSs;
        private bool _loading = true;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canEditMSs = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Services.Edit)).Succeeded;

            await GetDataAsync();
        }

        private async Task GetDataAsync()
        {
            _loading = true;

            var result = await MSManager.GetAllTenantsByService(MSId);
            if (result.Succeeded)
            {
                MudDialog.SetTitle($"Настройка организаций для {result.Response.ServiceName}");

                _serviceTenantList = result.Response.Tenants;
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

            var requestTenantList = new List<TenantServiceIds>();

            foreach (var item in _serviceTenantList.Where(x => x.Selected))
            {
                requestTenantList.Add(new TenantServiceIds() { TenantId = item.Id });
            }

            var result = await MSManager.UpdateTenantsByService(new RequestServiceTenant() { Tenants = requestTenantList }, MSId);

            if (result.Succeeded)
            {
                _snackBar.Add(result.Messages[0], Severity.Success);
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
        /// Функция поиска
        /// </summary>
        /// <param name="userRole"></param>
        /// <returns></returns>
        private bool Search(ResponseTenantService tenant)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;

            if (tenant.FullName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
                return true;

            if (tenant.INN?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
                return true;

            if (tenant.Email?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
                return true;

            return false;
        }

        /// <summary>
        /// Закрытие модального окна
        /// </summary>
        private void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
