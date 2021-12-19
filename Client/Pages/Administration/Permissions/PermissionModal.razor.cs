using MudBlazor;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Infrastructure.Models.Request;
using AuthClient.Client.Infrastructure.Managers.Identity.Permissions;

namespace AuthClient.Client.Pages.Administration.Permissions
{
    public partial class PermissionModal
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        [Inject] private IPermissionManager PermissionManager { get; set; }

        [Parameter] public string PermissionId { get; set; }

        private RequestPermission _permissionModel = new();
        private bool _loading = false;

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(PermissionId))
            {
                await LoadDataAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            _loading = true;

            var response = await PermissionManager.GetPermissionByIdAsync(PermissionId);

            if (!response.Succeeded)
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
                return;
            }

            _permissionModel.Type = response.Response.Type;
            _permissionModel.Value = response.Response.Value;

            _loading = false;
        }

        private async Task SubmitAsync()
        {
            _loading = true;

            IResult response;

            if (string.IsNullOrEmpty(PermissionId))
            {
                response = await PermissionManager.CreatePermissionAsync(_permissionModel);
            }
            else
            {
                response = await PermissionManager.UpdatePermissionAsync(_permissionModel, PermissionId);
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

        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
