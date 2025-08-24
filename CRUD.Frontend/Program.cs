using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace CRUD.Frontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            var url = "https://localhost:7002"; // Adjust the URL to match your backend API endpoint
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(url) });

            await builder.Build().RunAsync();
        }
    }
}
