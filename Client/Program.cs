using System.Threading.Tasks;
using AuthClient.Client.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace AuthClient.Client
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args).AddRootComponents().AddClientServices();

            var host = builder.Build();
            await Initialize(host);
            await host.RunAsync();
        }

        private static async Task Initialize(WebAssemblyHost host)
        {
            host.Services.GetService<NavigationExtensions>();
        }
    }
}