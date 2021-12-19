using Blazored.LocalStorage;
using AuthClient.Client.Infrastructure.Models.Request;
using AuthClient.Client.Infrastructure.Models.Response;
using AuthClient.Client.Infrastructure.Authentication;
using AuthClient.Client.Infrastructure.Extensions;
using AuthClient.Client.Infrastructure.Routes;
using AuthClient.Shared.Constants.Storage;
using AuthClient.Shared.Wrapper;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Components;

namespace AuthClient.Client.Infrastructure.Managers.Identity.Authentication
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly NavigationManager _navigationManager;

        public AuthenticationManager(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
            _navigationManager = navigationManager;
        }

        public async Task<ClaimsPrincipal> CurrentUser()
        {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return state.User;
        }

        public async Task<IResult> Login(RequestAuth model, string serviceId = null)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, AuthEndpoints.Get + "?Email=" + HttpUtility.UrlEncode(model.Email) + "&Password=" + HttpUtility.UrlEncode(model.Password) + "&ServiceId=" + serviceId);

            var response = await _httpClient.SendAsync(httpRequestMessage);

            var result = await response.ToResult<ResponseAuth>();

            if (result.Succeeded)
            {
                var token = result.Response.AccessToken;
                var refreshToken = result.Response.RefreshToken;

                await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, token);
                await _localStorage.SetItemAsync(StorageConstants.Local.RefreshToken, refreshToken);

                await ((MainStateProvider)this._authenticationStateProvider).StateChangedAsync();

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                if (!string.IsNullOrEmpty(result.Response.RedirectURL)) _navigationManager.NavigateTo(result.Response.RedirectURL + "?accessToken=" + result.Response.AccessToken, true);

                return await Result.SuccessAsync();
            }
            else
            {
                return await Result.FailAsync(result.Messages);
            }
        }

        public async Task<IResult> Logout()
        {
            var refreshToken = await _localStorage.GetItemAsync<string>(StorageConstants.Local.RefreshToken);

            HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, AuthEndpoints.Revoke + "?token=" + refreshToken);

            await _httpClient.SendAsync(httpRequestMessage);

            await _localStorage.RemoveItemAsync(StorageConstants.Local.AuthToken);
            await _localStorage.RemoveItemAsync(StorageConstants.Local.RefreshToken);

            ((MainStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();

            _httpClient.DefaultRequestHeaders.Authorization = null;

            return await Result.SuccessAsync();
        }

        public async Task<IResult> TryForceLogout()
        {
            await _localStorage.RemoveItemAsync(StorageConstants.Local.AuthToken);
            await _localStorage.RemoveItemAsync(StorageConstants.Local.RefreshToken);

            ((MainStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();

            _httpClient.DefaultRequestHeaders.Authorization = null;

            return await Result.SuccessAsync();
        }

        public async Task<string> RefreshToken()
        {
            var refreshToken = await _localStorage.GetItemAsync<string>(StorageConstants.Local.RefreshToken);

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, AuthEndpoints.Refresh + "?token=" + refreshToken);

            var response = await _httpClient.SendAsync(httpRequestMessage);

            var result = await response.ToResult<ResponseAuth>();

            if (!result.Succeeded)
            {
                throw new ApplicationException("При обновлении токена произошла ошибка");
            }

            var token = result.Response.AccessToken;
            refreshToken = result.Response.RefreshToken;

            await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, token);
            await _localStorage.SetItemAsync(StorageConstants.Local.RefreshToken, refreshToken);

            //await ((MainStateProvider)this._authenticationStateProvider).StateChangedAsync();

            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return token;
        }

        /// <summary>
        /// Проверяем не истек ли срок действия токена обновления
        /// </summary>
        /// <returns></returns>
        public async Task<string> TryRefreshToken()
        {
            //Проверяем токен
            var availableToken = await _localStorage.GetItemAsync<string>(StorageConstants.Local.RefreshToken);

            if (string.IsNullOrEmpty(availableToken)) return string.Empty;

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
            var timeUTC = DateTime.UtcNow;
            var diff = expTime - timeUTC;

            if (diff.TotalMinutes <= 1)
                return await RefreshToken();

            return string.Empty;
        }

        public async Task<string> TryForceRefreshToken()
        {
            return await RefreshToken();
        }

        public async Task<bool> CheckAccessToken()
        {
            //Проверяем токен
            var availableToken = await _localStorage.GetItemAsync<string>(StorageConstants.Local.AuthToken);

            if (string.IsNullOrEmpty(availableToken)) return false;

            //_httpClient.DefaultRequestHeaders.Authorization = null;

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, AuthEndpoints.Check + "?token=" + availableToken);

            var response = await _httpClient.SendAsync(httpRequestMessage);
            var result = await response.ToResult<bool>();

            if (!result.Succeeded)
            {
                return true;
            }

            return false;
        }
    }
}