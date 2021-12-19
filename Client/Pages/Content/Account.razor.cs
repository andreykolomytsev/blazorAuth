using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AuthClient.Shared.Constants.Permission;

namespace AuthClient.Client.Pages.Content
{
    public partial class Account
    {
        private ClaimsPrincipal _currentUser;

        private bool _canViewUsers;
        private bool _canViewTokens;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canViewUsers = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Users.View)).Succeeded;
            _canViewTokens = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Tokens.View)).Succeeded;
        }
    }
}
