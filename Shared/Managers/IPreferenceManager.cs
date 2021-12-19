using AuthClient.Shared.Settings;
using System.Threading.Tasks;

namespace AuthClient.Shared.Managers
{
    public interface IPreferenceManager
    {
        Task SetPreference(IPreference preference);

        Task<IPreference> GetPreference();
    }
}