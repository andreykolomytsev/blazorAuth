using MudBlazor;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using AuthClient.Shared.Constants.Permission;
using AuthClient.Client.Infrastructure.Models.Request;
using AuthClient.Client.Extensions;

namespace AuthClient.Client.Pages.Content.Components
{
    public partial class Profile
    {
        private readonly RequestUserUpdate _profileModel = new();

        private ClaimsPrincipal _currentUser;

        private char _firstLetterOfName;
        private string _tenantName;
        private string _email;
        private string UserId;

        private bool _canEditUser;
        private bool _loading = false;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canEditUser = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Users.Edit)).Succeeded;

            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            _loading = true;
            StateHasChanged();

            var state = await _stateProvider.GetAuthenticationStateAsync();
            UserId = state.User.GetUserId();

            var data = await _userManager.GetUserByIdAsync(UserId);

            if (data.Succeeded)
            {
                _profileModel.FirstName = data.Response.FirstName;
                _profileModel.LastName  = data.Response.LastName;
                _profileModel.MiddleName = data.Response.MiddleName;
                _profileModel.Email = _email = data.Response.Email;
                _profileModel.PhoneNumber = data.Response.PhoneNumber;
                _profileModel.IsActive = data.Response.IsActive;
                //_tenantName = data.Response.Tenant.FullName;

                if (_profileModel.FirstName.Length > 0)
                {
                    _firstLetterOfName = _profileModel.FirstName[0];
                }
            }
            else
            {
                foreach (var message in data.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }

            _loading = false;
            StateHasChanged();
        }

        private async Task UpdateProfileAsync()
        {
            _loading = true;

            var request = await _userManager.UpdateUserAsync(_profileModel, UserId);

            if (request.Succeeded)
            {
                _snackBar.Add(request.Messages[0], Severity.Success);
            }
            else
            {
                foreach (var message in request.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }

            _loading = false;
        }
    }
}