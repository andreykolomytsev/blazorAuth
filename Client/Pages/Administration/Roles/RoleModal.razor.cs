using MudBlazor;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Infrastructure.Models.Request;
using AuthClient.Client.Infrastructure.Managers.Identity.Roles;

namespace AuthClient.Client.Pages.Administration.Roles
{
    public partial class RoleModal
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        [Inject] private IRoleManager RoleManager { get; set; }

        [Parameter] public string RoleId { get; set; }

        private RequestRole _roleModel = new();
        private bool _loading = false;

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(RoleId))
            {
                await LoadDataAsync();
            }
        }

        /// <summary>
        /// Получить данные из API
        /// </summary>
        /// <returns></returns>
        private async Task LoadDataAsync()
        {
            _loading = true;

            var response = await RoleManager.GetRoleByIdAsync(RoleId);

            if (!response.Succeeded)
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
                return;
            }

            _roleModel.Name = response.Response.Name;
            _roleModel.Description = response.Response.Description;

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

            if (string.IsNullOrEmpty(RoleId))
            {
                response = await RoleManager.CreateRoleAsync(_roleModel);
            }
            else
            {
                response = await RoleManager.UpdateRoleAsync(_roleModel, RoleId);
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