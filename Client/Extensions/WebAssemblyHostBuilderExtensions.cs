using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Blazored.LocalStorage;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using AuthClient.Client.Infrastructure.Authentication;
using AuthClient.Client.Infrastructure.Managers;
using AuthClient.Client.Infrastructure.Managers.Preferences;
using AuthClient.Shared.Constants.Permission;

namespace AuthClient.Client.Extensions
{
    public static class WebAssemblyHostBuilderExtensions
    {
        private const string ClientName = "AuthClient";

        public static WebAssemblyHostBuilder AddRootComponents(this WebAssemblyHostBuilder builder)
        {
            builder.RootComponents.Add<App>("#app");

            return builder;
        }

        public static WebAssemblyHostBuilder AddClientServices(this WebAssemblyHostBuilder builder)
        {
            builder.Services
                .AddAuthorizationCore(options =>
                {
                    RegisterPermissionClaims(options);
                })
                .AddBlazoredLocalStorage()
                .AddMudServices(configuration =>
                {
                    configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

                    configuration.SnackbarConfiguration.PreventDuplicates = false;
                    configuration.SnackbarConfiguration.NewestOnTop = false;
                    configuration.SnackbarConfiguration.ShowCloseIcon = true;
                    configuration.SnackbarConfiguration.ClearAfterNavigation = false;

                    configuration.SnackbarConfiguration.VisibleStateDuration = 10000;
                    configuration.SnackbarConfiguration.HideTransitionDuration = 500;
                    configuration.SnackbarConfiguration.ShowTransitionDuration = 500;

                    configuration.SnackbarConfiguration.MaxDisplayedSnackbars = 4;
                })
                .AddSingleton<NavigationExtensions>()
                .AddScoped<ClientPreferenceManager>()
                .AddScoped<MainStateProvider>()
                .AddScoped<AuthenticationStateProvider, MainStateProvider>()
                .AddManagers()
                .AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(ClientName).EnableIntercept(sp))
                .AddHttpClient(ClientName, client =>
                {
#if DEBUG
                    client.BaseAddress = new Uri("https://localhost:9001/api/"); // Адрес API
#else
                    client.BaseAddress = new Uri("https://localhost:9001/api/"); // Адрес API
#endif
                });

            builder.Services.AddHttpClientInterceptor();
            return builder;
        }

        /// <summary>
        /// Регистрируем ManagerService
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            var managers = typeof(IManager);

            var types = managers.Assembly.GetExportedTypes().Where(t => t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Service = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                })
                .Where(t => t.Service != null);

            foreach (var type in types)
            {
                if (managers.IsAssignableFrom(type.Service))
                {
                    services.AddTransient(type.Service, type.Implementation);
                }
            }

            return services;
        }

        private static void RegisterPermissionClaims(AuthorizationOptions options)
        {
            foreach (var prop in typeof(AllPermissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                {
                    options.AddPolicy(propertyValue.ToString(), policy => policy.RequireClaim(PermissionConstants.Permission, propertyValue.ToString()));
                }
            }
        }
    }
}