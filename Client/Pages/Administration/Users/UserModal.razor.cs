using MudBlazor;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Extensions;
using AuthClient.Client.Infrastructure.Models.Request;
using AuthClient.Client.Infrastructure.Models.Response;
using AuthClient.Shared.Constants.Permission;

namespace AuthClient.Client.Pages.Administration.Users
{
    public partial class UserModal
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        [Inject] private IJSRuntime JS { get; set; }

        [Parameter] public string UserId { get; set; }

        private ClaimsPrincipal _currentUser;

        private RequestUser _userCreateModel = new();

        private RequestUserUpdate _userUpdateModel = new();

        private ResponseTenant _tenant = new();

        private bool _canViewTenants;

        private bool _isAddMode = true;

        private bool _loading = false;


        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canViewTenants = (await _authorizationService.AuthorizeAsync(_currentUser, AllPermissions.Tenants.View)).Succeeded;
            if (!string.IsNullOrEmpty(UserId)) _isAddMode = false;

            await LoadDataAsync();
        }

        /// <summary>
        /// Загрузка данных и заполнение формы
        /// </summary>
        /// <returns></returns>
        private async Task LoadDataAsync()
        {
            _loading = true;

            if (!_isAddMode) // мод на редактирование
            {
                var response = await GetUser(UserId);
                if (response != null)
                {
                    _userUpdateModel.Email = response.Response.Email;
                    _userUpdateModel.FirstName = response.Response.FirstName;
                    _userUpdateModel.LastName = response.Response.LastName;
                    _userUpdateModel.MiddleName = response.Response.MiddleName;
                    _userUpdateModel.PhoneNumber = response.Response.PhoneNumber;
                    _userUpdateModel.IsActive = response.Response.IsActive;

                    //_tenant = response.Response.Tenant;
                }
            }
            else
            {
                //var state = await _stateProvider.GetAuthenticationStateAsync();

                //var response = await GetUser(state.User.GetUserId());
                //if (response != null)
                //{
                //    _tenant = response.Response.Tenant;
                //}
            }

            _loading = false;
        }

        /// <summary>
        /// Получить данные пользователя по ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<IResult<ResponseUser>> GetUser(string userId)
        {
            var response = await _userManager.GetUserByIdAsync(userId);

            if (!response.Succeeded)
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
                return null;
            }

            return response;
        }

        /// <summary>
        /// Подтвердить форму
        /// </summary>
        /// <returns></returns>
        private async Task SubmitAsync()
        {
            _loading = true;

            IResult response;

            if (_isAddMode)
            {
                _userCreateModel.TenantId = _tenant.Id;
                response = await _userManager.CreateUserAsync(_userCreateModel);
            }
            else
            {
                _userUpdateModel.TenantId = _tenant.Id;
                response = await _userManager.UpdateUserAsync(_userUpdateModel, UserId);
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
        /// Модальное окно с организациями
        /// </summary>
        /// <returns></returns>
        private async Task InvokeTenantModal()
        {
            var isMobileDevice = await JS.InvokeAsync<bool>("isMobile");

            var parameters = new DialogParameters();
          
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Large, FullWidth = true, DisableBackdropClick = true, FullScreen = isMobileDevice };
            var dialog = _dialogService.Show<SelectTenantModal>("Доступные организации", parameters, options);
            
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                _tenant = (ResponseTenant)result.Data;
            }
        }

        /// <summary>
        /// Закрыть текущее модальное окно
        /// </summary>
        private void Cancel()
        {
            MudDialog.Cancel();
        }

        #region PasswordField
        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        private void TogglePasswordVisibility()
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
        #endregion
    }
}
