using MudBlazor;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Infrastructure.Models.Request;
using AuthClient.Client.Infrastructure.Managers.Identity.MicroServices;

namespace AuthClient.Client.Pages.Administration.MicroServices
{
    public partial class MicroServicesModal
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        [Inject] private IMSManager MSManager { get; set; }

        [Parameter] public string MSId { get; set; }

        private RequestMS _MSModel = new();
        private bool _loading = false;

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(MSId))
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

            var response = await MSManager.GetMSByIdAsync(MSId);

            if (!response.Succeeded)
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
                return;
            }

            _MSModel.FullName = response.Response.FullName;
            _MSModel.Description = response.Response.Description;
            _MSModel.URL = response.Response.URL;
            _MSModel.IP = response.Response.IP;
            _MSModel.Port = response.Response.Port;

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

            if (string.IsNullOrEmpty(MSId))
            {
                response = await MSManager.CreateMSAsync(_MSModel);
            }
            else
            {
                response = await MSManager.UpdateMSAsync(_MSModel, MSId);
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
