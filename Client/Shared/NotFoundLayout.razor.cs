using MudBlazor;
using System.Threading.Tasks;
using AuthClient.Client.Infrastructure.Settings;

namespace AuthClient.Client.Shared
{
    public partial class NotFoundLayout
    {
        private MudTheme _currentTheme;

        protected override async Task OnInitializedAsync()
        {
            _currentTheme = MainTheme.DefaultTheme;
            _currentTheme = await _clientPreferenceManager.GetCurrentThemeAsync();
        }
    }
}
