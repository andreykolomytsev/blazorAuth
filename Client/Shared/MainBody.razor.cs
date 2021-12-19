using MudBlazor;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using AuthClient.Client.Extensions;
using AuthClient.Shared.Constants.Permission;

namespace AuthClient.Client.Shared
{
    public partial class MainBody
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback OnDarkModeToggle { get; set; }

        [Parameter]
        public EventCallback<bool> OnRightToLeftToggle { get; set; }

        private DrawerVariant DrawerVariant { get; set; } = DrawerVariant.Mini;

        [Inject] private IJSRuntime JS { get; set; }

        private ClaimsPrincipal _currentUser;

        private bool _drawerOpen = true;
        private string CurrentUserId { get; set; }
        private string FirstName { get; set; }
        private string Email { get; set; }
        private char FirstLetterOfName { get; set; }

        private bool _canViewUsers;
        private bool _canViewTokens;

        protected override async Task OnInitializedAsync()
        {
            var isMobileDevice = await JS.InvokeAsync<bool>("isMobile");
            if (isMobileDevice)
                DrawerVariant = DrawerVariant.Responsive;

            _currentUser = await _authenticationManager.CurrentUser();
            _canViewUsers = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Users.View)).Succeeded;
            _canViewTokens = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Tokens.View)).Succeeded;

            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (user == null) return;
            if (user.Identity?.IsAuthenticated == true)
            {
                if (string.IsNullOrEmpty(CurrentUserId))
                {
                    Email = user.GetEmail();
                    CurrentUserId = user.GetUserId();
                    FirstName = user.GetFirstName();
                    if (FirstName.Length > 0)
                    {
                        FirstLetterOfName = FirstName[0];
                    }
                }
            }
        }

        public async Task ToggleDarkMode()
        {
            await OnDarkModeToggle.InvokeAsync();
        }

        private void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        private void ProfileClick()
        {
            _navigationManager.NavigateTo("/account");
        }

        private void LogoutClick()
        {
            var parameters = new DialogParameters
            {
                {nameof(Dialogs.Logout.ContentText), "Вы уверены, что хотите выйти"},
                {nameof(Dialogs.Logout.ButtonText), "Выйти"},
                {nameof(Dialogs.Logout.Color), Color.Error}
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            _dialogService.Show<Dialogs.Logout>("Выход из системы", parameters, options);
        }
    }
}