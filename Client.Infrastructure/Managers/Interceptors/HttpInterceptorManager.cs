using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Toolbelt.Blazor;
using AuthClient.Client.Infrastructure.Managers.Identity.Authentication;

namespace AuthClient.Client.Infrastructure.Managers.Interceptors
{
    public class HttpInterceptorManager : IHttpInterceptorManager
    {
        private readonly HttpClientInterceptor _interceptor;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly NavigationManager _navigationManager;
        private readonly ISnackbar _snackBar;

        public HttpInterceptorManager(HttpClientInterceptor interceptor, IAuthenticationManager authenticationManager, NavigationManager navigationManager, ISnackbar snackBar)
        {
            _interceptor = interceptor;
            _authenticationManager = authenticationManager;
            _navigationManager = navigationManager;
            _snackBar = snackBar;
        }

        public void RegisterEvent() => _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;

        public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            var absPath = e.Request.RequestUri.AbsolutePath;

            bool tokenNeedUpdate = false;

            // перед каждым запросом проверяем не нужно ли обновить токен
            if (absPath != "/api/authenticate/refreshtoken")
            {
                //перед каждым запросом проверяем действительный ли токен доступа
                if (absPath != "/api/authenticate/checktoken")
                {
                    tokenNeedUpdate = await _authenticationManager.CheckAccessToken();
                }

                try
                {
                    var token = await _authenticationManager.TryRefreshToken();

                    if (!string.IsNullOrEmpty(token))
                    {
                        _snackBar.Add("Токен обновлен", Severity.Success);

                        // добавляем обновленный токен доступа в заголовки запросов
                        e.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    else
                    {
                        if (tokenNeedUpdate)
                        {
                            token = await _authenticationManager.TryForceRefreshToken();

                            if (!string.IsNullOrEmpty(token))
                            {
                                _snackBar.Add("У Вас обновились права доступа. Обновите страницу для получения актуальных данных", Severity.Warning,
                                    new Action<SnackbarOptions>(sb =>
                                    {
                                        sb.RequireInteraction = true;
                                        sb.ShowCloseIcon = false;
                                        sb.Onclick = snackbar => { _navigationManager.NavigateTo("/", true); return Task.CompletedTask; };
                                    }));

                                // добавляем обновленный токен доступа в заголовки запросов
                                e.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _snackBar.Add("Авторизуйтесь еще раз", Severity.Error);
                    await _authenticationManager.TryForceLogout();
                    _navigationManager.NavigateTo("/");
                }
            }
        }

        public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
    }
}