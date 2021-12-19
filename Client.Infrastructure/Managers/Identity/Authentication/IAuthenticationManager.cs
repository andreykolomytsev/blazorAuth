using System.Security.Claims;
using System.Threading.Tasks;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Infrastructure.Models.Request;

namespace AuthClient.Client.Infrastructure.Managers.Identity.Authentication
{
    public interface IAuthenticationManager : IManager
    {
        Task<IResult> Login(RequestAuth model, string serviceId = null);

        Task<IResult> Logout();

        Task<IResult> TryForceLogout();

        Task<string> RefreshToken();

        Task<string> TryRefreshToken();

        Task<string> TryForceRefreshToken();

        Task<ClaimsPrincipal> CurrentUser();

        Task<bool> CheckAccessToken();
    }
}