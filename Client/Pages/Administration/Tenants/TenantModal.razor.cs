using MudBlazor;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Infrastructure.Models.Request;
using AuthClient.Client.Infrastructure.Managers.Identity.Tenants;

namespace AuthClient.Client.Pages.Administration.Tenants
{
    public partial class TenantModal
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        [Inject] private ITenantManager TenantManager { get; set; }

        [Parameter] public string TenantId { get; set; }

        private RequestTenant _tenantModel = new();
        private bool _loading = false;

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(TenantId))
            {
                await LoadDataAsync();
            }
        }

        /// <summary>
        /// Загрузка данных из API
        /// </summary>
        /// <returns></returns>
        private async Task LoadDataAsync()
        {
            _loading = true;

            var response = await TenantManager.GetTenantByIdAsync(TenantId);

            if (!response.Succeeded)
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
                return;
            }

            _tenantModel.FullName = response.Response.FullName;
            _tenantModel.INN = response.Response.INN;
            _tenantModel.KPP = response.Response.KPP;
            _tenantModel.OGRN = response.Response.OGRN;
            _tenantModel.OKPO = response.Response.OKPO;
            _tenantModel.Email = response.Response.Email;
            _tenantModel.Phone = response.Response.Phone;
            _tenantModel.Address = response.Response.Address;

            _loading = false;
        }

        /// <summary>
        /// Отправка формы
        /// </summary>
        /// <returns></returns>
        private async Task SubmitAsync()
        {
            _loading = true;

            IResult response;

            if (string.IsNullOrEmpty(TenantId))
            {
                response = await TenantManager.CreateTenantAsync(_tenantModel);
            }
            else
            {
                response = await TenantManager.UpdateTenantAsync(_tenantModel, TenantId);
            }

            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }

            _loading = false;
        }

        /// <summary>
        /// Закрыть текущее модальное окно
        /// </summary>
        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
