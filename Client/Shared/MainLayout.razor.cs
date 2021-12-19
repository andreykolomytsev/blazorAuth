using AuthClient.Client.Infrastructure.Settings;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace AuthClient.Client.Shared
{
    public partial class MainLayout : IDisposable
    {
        private MudTheme _currentTheme;

        protected override async Task OnInitializedAsync()
        {
            _currentTheme = MainTheme.DefaultTheme;
            _currentTheme = await _clientPreferenceManager.GetCurrentThemeAsync();
            _interceptor.RegisterEvent();
        }

        private async Task DarkMode()
        {
            bool isDarkMode = await _clientPreferenceManager.ToggleDarkModeAsync();
            _currentTheme = isDarkMode ? MainTheme.DefaultTheme : MainTheme.DarkTheme;
        }

        public void Dispose()
        {
            _interceptor.DisposeEvent();
        }
    }
}