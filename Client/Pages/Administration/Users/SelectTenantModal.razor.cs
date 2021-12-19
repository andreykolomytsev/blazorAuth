using AuthClient.Client.Infrastructure.Models.Response;
using AuthClient.Client.Infrastructure.Managers.Identity.Tenants;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AuthClient.Client.Pages.Administration.Users
{
    public partial class SelectTenantModal
    {
        [CascadingParameter] private MudDialogInstance MudSelectTenantDialog { get; set; }

        [Inject] private ITenantManager TenantManager { get; set; }

        private List<ResponseTenant> _tenantList = new();

        /// <summary>
        /// Загрузка данных в таблицу
        /// </summary>
        private async Task<TableData<ResponseTenant>> ServerReloadAsync(TableState state)
        {
            var response = await TenantManager.GetAllTenantsAsync(state.Page + 1, state.PageSize);

            if (!response.Succeeded) return new TableData<ResponseTenant>() { TotalItems = 0, Items = _tenantList };

            _tenantList = response.Response;

            return new TableData<ResponseTenant>() { TotalItems = response.TotalCount, Items = _tenantList };
        }

        /// <summary>
        /// Выбор организации
        /// </summary>
        /// <param name="responseTenant"></param>
        private void Confirm(ResponseTenant responseTenant)
        {
            MudSelectTenantDialog.Close(DialogResult.Ok(responseTenant));
        }
    }
}
