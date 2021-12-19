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

namespace AuthClient.Client.Pages.Administration.Users
{
    public partial class UserRolesModal
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        [Parameter] public string UserId { get; set; }

        [Parameter] public string User { get; set; }

        public List<UserRoleModel> UserRolesList = new();

        private string _searchString = "";

        private ClaimsPrincipal _currentUser;

        private bool _canEditUsers;
        private bool _loading = true;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canEditUsers = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Users.Edit)).Succeeded;

            var response = await _userManager.GetUserRolesAsync(UserId);
            if (response.Succeeded)
            {
                UserRolesList = response.Response.UserRoles;

                MudDialog.SetTitle($"Настройка ролей для {User}");
            }

            _loading = false;
        }

        /// <summary>
        /// Отправка формы
        /// </summary>
        /// <returns></returns>
        private async Task SaveAsync()
        {
            _loading = true;

            var request = new RequestUpdateUserRoles()
            {
                UserId = UserId,
                UserRoles = UserRolesList.Where(x => x.Selected).ToList()
            };

            var result = await _userManager.UpdateUserRolesAsync(request);
            if (result.Succeeded)
            {
                _snackBar.Add(result.Messages[0], Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
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
        private bool Search(UserRoleModel userRole)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;

            if (userRole.RoleName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

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
