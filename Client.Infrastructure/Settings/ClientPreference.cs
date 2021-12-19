using AuthClient.Shared.Settings;

namespace AuthClient.Client.Infrastructure.Settings
{
    /// <summary>
    /// Настройки клиента UI
    /// </summary>
    public record ClientPreference : IPreference
    {
        public bool IsDarkMode { get; set; }
        public bool IsDrawerOpen { get; set; }
        public string PrimaryColor { get; set; }
    }
}