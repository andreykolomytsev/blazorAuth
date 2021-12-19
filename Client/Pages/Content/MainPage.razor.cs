using MudBlazor;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using AuthClient.Shared.Helpers;
using AuthClient.Shared.Wrapper;
using AuthClient.Client.Infrastructure.Models.Response;
using AuthClient.Client.Infrastructure.Managers.Identity.MicroServices;

namespace AuthClient.Client.Pages.Content
{
    public partial class MainPage
    {
        private readonly List<BreadcrumbItem> _items = new()
        {
            new BreadcrumbItem("", href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem("Доступ к микросервисам", href: null, disabled: true),
        };

        [Inject] private IMSManager MSManager { get; set; }

        private bool HasNextPage = true;


        /// <summary>
        /// Получить данные из API
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private async Task<PaginatedResult<ResponseMS>> GetDataAsync(int page, int pageSize)
        {
            return await MSManager.GetAllMSsAsync(page, pageSize);
        }

        async Task<IEnumerable<ResponseMS>> GetItems(InfiniteScrollingItemsProviderRequest request)
        {
            var data = await GetDataAsync(request.StartIndex, 50);

            HasNextPage = false;

            if (data.HasNextPage)
                HasNextPage = true;

            StateHasChanged();

            return data.Response;
        }

        private void Redirect(MouseEventArgs e, string url)
        {
            _navigationManager.NavigateTo(url);
        }
    }
}
