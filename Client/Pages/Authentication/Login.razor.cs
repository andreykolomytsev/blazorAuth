using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using AuthClient.Client.Infrastructure.Models.Request;

namespace AuthClient.Client.Pages.Authentication
{
    public partial class Login
    {
        private EditContext editContext;
        private bool Validated { get; set; } = false;

        private RequestAuth _authModel = new();

        private bool _loading = false;

        protected override async Task OnInitializedAsync()
        {
            editContext = new(_authModel);
            editContext.OnFieldChanged += EditContext_OnFieldChanged;

            var currentUser = await _authenticationManager.CurrentUser();

            if (currentUser.Identity.IsAuthenticated)
            {
                _navigationManager.NavigateTo("/");
            }
        }

        private void EditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            SetOkDisabledStatus();
        }

        private void SetOkDisabledStatus()
        {
            if (editContext.Validate())
            {
                Validated = true;
            }
            else
            {
                Validated = false;
            }
        }

        private async Task SubmitAsync()
        {
            _loading = true;

            string serviceId = null;

            //if (_navigationManager.Uri.Contains("?redirectURL"))
            //{
            //    var redirect = _navigationManager.Uri.Split('=');
            //    if (redirect.Length == 2)
            //    {
            //        serviceId = redirect[1];
            //    }
            //}

            var result = await _authenticationManager.Login(_authModel, serviceId);

            if (!result.Succeeded)
            {
                foreach (var message in result.Messages)
                {
                    _snackBar.Add(message, Severity.Error, new System.Action<SnackbarOptions>(sb => sb.CloseAfterNavigation = true));
                }
            }

            _loading = false;
        }

        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        void TogglePasswordVisibility()
        {
            if (_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;
            }
        }

        private void FillAdministratorCredentials()
        {
            _authModel.Email = "admin@gmail.com";
            _authModel.Password = "Qwerty1234$!";

            SetOkDisabledStatus();
        }
    }
}